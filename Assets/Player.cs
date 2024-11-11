using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject destroyItemEffect;
    [SerializeField] private GameObject destroyEnemyEffect;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fireRateMin = 0.25f;
    [SerializeField] private float fireRateMax = 0.5f;
    [SerializeField] private int maxProjectiles = 10;

    [SerializeField] private LayerMask projectileHitsMask; // Layer mask to specify which layers the raycast can hit

    [SerializeField] private GameManager gameManager;
    [SerializeField] private int life = 100;
    [SerializeField] private int PowerupLifeRegen = 10;
    [SerializeField] private int maxLife = 100;
    [SerializeField] private bool hasArmor;

    private float fireTimer;
    [SerializeField] private int currentProjectiles = 10;
    [SerializeField] private int PowerupProjectiles = 5;
    [SerializeField] private int projectilesBurst = 25;
    [SerializeField] private float burstDelay = 0.1f; // Delay between projectiles in a burst
    private GameObject[] projectiles;
    private int currentProjectileIndex = 0;

    public int CurrentProjectiles {  get { return currentProjectiles; } }
    public int Life { get { return life; } }
    public bool HasArmor { get { return hasArmor; } }

    [Header("Audios")]
    [SerializeField] private AudioClip[] fireClips;
    [SerializeField] private AudioClip[] powerupClips;
    [SerializeField] private AudioClip[] hitClips;

    void Start()
    {
        fireTimer = Random.Range(fireRateMin, fireRateMax);
        projectiles = new GameObject[maxProjectiles];

        for (int i = 0; i < maxProjectiles; i++)
        {
            projectiles[i] = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            projectiles[i].SetActive(false);
        }
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            AttemptToFire();
            fireTimer = Random.Range(fireRateMin, fireRateMax);
        }
    }

    void AttemptToFire()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy == null || currentProjectiles <= 0) return;

        Vector3 directionToEnemy = (closestEnemy.transform.position - projectileSpawnPoint.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(projectileSpawnPoint.position, directionToEnemy, Mathf.Infinity, projectileHitsMask);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            StartCoroutine(FireBurst(directionToEnemy));
            currentProjectiles--;
        }
    }

    IEnumerator FireBurst(Vector3 direction)
    {
        for (int i = 0; i < projectilesBurst && currentProjectiles > 0; i++)
        {
            FireProjectile(direction);
            yield return new WaitForSeconds(burstDelay); // Delay between each projectile in the burst
        }
    }

    void FireProjectile(Vector3 direction)
    {
        // Apply a random spread angle within �45 degrees
        float randomAngle = Random.Range(-45f, 45f);
        Quaternion spreadRotation = Quaternion.Euler(0, 0, randomAngle);

        // Adjust the direction with the spread rotation
        Vector3 spreadDirection = spreadRotation * direction;

        // Set up the projectile
        GameObject projectile = projectiles[currentProjectileIndex];
        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, spreadDirection);
        projectile.SetActive(true);

        projectile.GetComponent<Projectile>().Manager = gameManager;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = spreadDirection * 5f; // Adjust speed as needed

        currentProjectileIndex = (currentProjectileIndex + 1) % maxProjectiles;

        MusicManager.Instance.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)], Random.Range(0.9f, 1.15f));
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }
        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is on the "Damage" layer
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Try to get the DamageHit component from the collided object
            DamageHit damageHit = collision.gameObject.GetComponentInParent<DamageHit>();

            if (damageHit != null)
            {
                int damageAmount = damageHit.GetDamageAmount();

                // Check if the player has armor
                if (hasArmor)
                {
                    // Disable armor without taking damage
                    hasArmor = false;
                    Debug.Log("Shield absorbed the damage and is now disabled.");
                }
                else
                {
                    // Apply damage to life
                    life -= damageAmount;
                    Debug.Log($"Player took {damageAmount} damage, remaining life: {life}");

                    // Check if life has reached zero or below
                    if (life <= 0)
                    {
                        GameOver();
                    }
                }
            }

            Instantiate(destroyEnemyEffect, collision.transform.parent.position, Quaternion.identity, collision.transform.parent.parent);
            Destroy(collision.transform.parent.gameObject);

            MusicManager.Instance.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)]);
        }
        else if (collision.gameObject.CompareTag("PowerupShield"))
        {
            hasArmor = true;
            Instantiate(destroyItemEffect, collision.transform.parent.position, Quaternion.identity, collision.transform.parent.parent);
            Destroy(collision.transform.parent.gameObject);

            MusicManager.Instance.PlayOneShot(powerupClips[Random.Range(0, powerupClips.Length)]);
        }
        else if (collision.gameObject.CompareTag("PowerupLife"))
        {
            life = Mathf.Min(life += PowerupLifeRegen, maxLife);
            Instantiate(destroyItemEffect, collision.transform.parent.position, Quaternion.identity, collision.transform.parent.parent);
            Destroy(collision.transform.parent.gameObject);

            MusicManager.Instance.PlayOneShot(powerupClips[Random.Range(0, powerupClips.Length)]);
        }
        else if (collision.gameObject.CompareTag("PowerupProjectile"))
        {
            currentProjectiles = Mathf.Min(currentProjectiles += PowerupProjectiles, maxProjectiles);
            Instantiate(destroyItemEffect, collision.transform.parent.position, Quaternion.identity, collision.transform.parent.parent);
            Destroy(collision.transform.parent.gameObject);

            MusicManager.Instance.PlayOneShot(powerupClips[Random.Range(0, powerupClips.Length)]);
        }
    }

    public void GameOver()
    {
        gameManager.GameOver();
    }
}

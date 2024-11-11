using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] platformPrefabs;       // Platform prefabs to generate
    [SerializeField] GameObject[] enemyPrefabsList;      // Enemy prefabs to spawn
    [SerializeField] GameObject[] powerupPrefabsList;    // Power-up prefabs to spawn
    [SerializeField] float spawnInterval = 2f;           // Interval between each platform generation
    [SerializeField] float platformSpeed = 2f;           // Speed at which each platform moves up
    [SerializeField] float platformLifetime = 10f;       // Time before the platform is destroyed
    [SerializeField] float enemySpawnChance = 0.2f;      // 1 in 5 chance to spawn an enemy
    [SerializeField] float powerupSpawnChance = 0.2f;    // 1 in 5 chance to spawn a power-up
    [SerializeField] float enemyHorizontalSpawnRange = 0.65f; // Horizontal range for enemy spawn position
    [SerializeField] float powerupHorizontalSpawnRange = 0.65f; // Horizontal range for power-up spawn position

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnPlatform();
            spawnTimer = 0f;
        }
    }

    void SpawnPlatform()
    {
        // Spawn a platform at the generator's position
        if (platformPrefabs.Length == 0) return;
        Vector3 spawnPosition = transform.position;
        GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], spawnPosition, Quaternion.identity);

        // Add the PlatformMover component to control upward movement
        platform.AddComponent<PlatformMover>().speed = platformSpeed;

        // Destroy the platform after a certain time to avoid clutter
        Destroy(platform, platformLifetime);

        // Enemy spawning
        if (enemyPrefabsList.Length > 0 && Random.value < enemySpawnChance)
        {
            GameObject enemyPrefab = enemyPrefabsList[Random.Range(0, enemyPrefabsList.Length)];
            if (enemyPrefab != null)
            {
                // Determine a random horizontal offset for the enemy
                float randomXOffset = Random.Range(-enemyHorizontalSpawnRange, enemyHorizontalSpawnRange);
                Vector3 enemySpawnPosition = spawnPosition + new Vector3(randomXOffset, 0f, 0f);

                // Instantiate the enemy at the calculated position
                Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity, platform.transform);
            }
        }

        // Power-up spawning
        if (powerupPrefabsList.Length > 0 && Random.value < powerupSpawnChance)
        {
            GameObject powerupPrefab = powerupPrefabsList[Random.Range(0, powerupPrefabsList.Length)];
            if (powerupPrefab != null)
            {
                // Determine a random horizontal offset for the power-up
                float randomXOffset = Random.Range(-powerupHorizontalSpawnRange, powerupHorizontalSpawnRange);
                Vector3 powerupSpawnPosition = spawnPosition + new Vector3(randomXOffset, 0.2f, 0f);

                // Instantiate the power-up at the calculated position
                 Instantiate(powerupPrefab, powerupSpawnPosition, Quaternion.identity, platform.transform);
            }
        }
    }
}

public class PlatformMover : MonoBehaviour
{
    public float speed;

    void Update()
    {
        // Move the platform upwards
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}

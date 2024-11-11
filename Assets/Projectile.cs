using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int scoreGained = 10;
    [SerializeField] private GameObject destroyEffect;
    public GameManager Manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Instantiate(destroyEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Manager.AddScore(scoreGained);
        }
    }

}

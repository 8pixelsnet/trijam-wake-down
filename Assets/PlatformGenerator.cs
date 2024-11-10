using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] platformPrefabs;      // The platform prefab to generate
    [SerializeField] float spawnInterval = 2f;       // Interval between each platform generation
    [SerializeField] float platformSpeed = 2f;       // Speed at which each platform moves up
    [SerializeField] float platformLifetime = 10f;   // Time before the platform is destroyed
    // [SerializeField] float horizontalSpawnRange = 2f; // Range for horizontal position variation

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
        // Determine a random horizontal offset
        // float randomXOffset = Random.Range(-horizontalSpawnRange, horizontalSpawnRange);
        // Vector3 spawnPosition = transform.position + new Vector3(randomXOffset, 0f, 0f);
        Vector3 spawnPosition = transform.position;

        // Instantiate a new platform at the calculated position
        if (platformPrefabs.Length == 0) {
            return;
        }
        GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], spawnPosition, Quaternion.identity);

        // Add the PlatformMover component to control upward movement
        platform.AddComponent<PlatformMover>().speed = platformSpeed;

        // Destroy the platform after a certain time to avoid clutter
        Destroy(platform, platformLifetime);
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

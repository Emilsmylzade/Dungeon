
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Array of enemy prefabs to spawn
    public GameObject[] enemyPrefabs;
    
    // Maximum number of enemies to spawn in the room
    public int maxEnemies = 10;

    // Reference to the RoomGenerator to get room layout information
    private RoomGenerator roomGenerator;
    public static EnemySpawner Instance { get; private set; }

    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Get the RoomGenerator component from the current room
        roomGenerator = GetComponent<RoomGenerator>();

        if (roomGenerator == null)
        {
            Debug.LogError("RoomGenerator component not found on the EnemySpawner GameObject.");
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy prefabs are not assigned in the EnemySpawner.");
            return;
        }
        
        // Spawn enemies when the room is initialized
        SpawnEnemies();
    }

    // Function to spawn enemies in the room
    private void SpawnEnemies()
    {
        // Get the number of enemies to spawn based on the room's difficulty
        int enemyCount = CalculateEnemyCount();

        for (int i = 0; i < enemyCount; i++)
        {
            // Randomly select an enemy prefab
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Get a random position within the room's layout
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Instantiate the enemy at the calculated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Function to calculate the number of enemies to spawn based on player progression
    private int CalculateEnemyCount()
    {
        // Example logic: Adjust the count based on player level or other metrics
        // This is a placeholder; implement your own logic based on player progression
        return Mathf.Clamp(maxEnemies, 1, maxEnemies);
    }

    // Function to get a random spawn position within the room
    private Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the room from the RoomGenerator
        //Bounds roomBounds = roomGenerator.GetRoomBounds();

        // Generate a random position within the room's bounds
        float x = 4;//Random.Range(roomBounds.min.x, roomBounds.max.x);
        float y = 4;//Random.Range(roomBounds.min.y, roomBounds.max.y);

        return new Vector3(x, y, 0); // Assuming a 2D top-down perspective
    }
}

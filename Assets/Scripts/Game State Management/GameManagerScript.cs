using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Player's current health
    public int playerHealth = 100;
    // Track if the game is currently running
    private bool isGameActive = true;

    // Event to trigger when the game is reset
    public delegate void GameReset();
    public static event GameReset OnGameReset;

    // Event to trigger when the game is over
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    // Singleton Instance
    public static GameManagerScript Instance { get; private set; }

    // Track the number of enemies defeated
    private int enemiesDefeated = 0;

    private void Awake()
    {
        // Implement Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes if needed
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Duplicate GameManagerScript instance found and destroyed.");
        }
    }

    private void Start()
    {
        // Initialize game state
        ResetGame();
    }

    void Update()
    {
        // Check for win/lose conditions
        CheckGameConditions();
    }

    // Method to check win/lose conditions
    private void CheckGameConditions()
    {
        if (playerHealth <= 0)
        {
            HandleGameOver();
        }
        // Additional win condition checks can be added here
        // Example win condition: if playerHealth is above a certain threshold
        if (playerHealth > 100) // Example condition
        {
            Debug.Log("Player has won the game!");
            HandleGameOver(); // End the game if the win condition is met
        }
    }

    // Method to handle game over scenario
    private void HandleGameOver()
    {
        isGameActive = false;
        OnGameOver?.Invoke();
        Debug.Log("Game Over! Player has died.");
        // Reset the game state after a delay or user input
        Invoke("ResetGame", 2f);
    }

    // Method to reset the game state
    public void ResetGame()
    {
        playerHealth = 100; // Reset player health
        isGameActive = true; // Set game active
        enemiesDefeated = 0; // Reset enemies defeated
        OnGameReset?.Invoke();
        Debug.Log("Game has been reset. Starting a new run.");
    }

    // Method to simulate player taking damage
    public void TakeDamage(int damage)
    {
        if (isGameActive)
        {
            playerHealth -= damage;
            Debug.Log($"Player took {damage} damage. Current health: {playerHealth}");
        }
    }

    // Method to simulate player healing
    public void Heal(int amount)
    {
        if (isGameActive)
        {
            playerHealth += amount;
            Debug.Log($"Player healed for {amount}. Current health: {playerHealth}");
        }
    }

    /// <summary>
    /// Increments the count of enemies defeated.
    /// </summary>
    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
        Debug.Log($"Enemies Defeated: {enemiesDefeated}");
    }

    /// <summary>
    /// Retrieves the current progression level based on enemies defeated.
    /// </summary>
    /// <returns>Number of enemies defeated.</returns>
    public int GetCurrentProgressionLevel()
    {
        return enemiesDefeated;
    }
}

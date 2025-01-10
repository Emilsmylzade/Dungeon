using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth;  // Current health of the enemy

    [Header("Death Settings")]
    public GameObject deathEffect; // Optional: Effect to play upon death (e.g., explosion)
    // Removed deathDelay to ensure immediate destruction

    [Header("Components")]
    private BigRobotAI bigRobotAI; // Reference to the BigRobotAI script

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health

        // Get reference to BigRobotAI if attached
        bigRobotAI = GetComponent<BigRobotAI>();
        if (bigRobotAI == null)
        {
            Debug.LogWarning("BigRobotAI script not found on the enemy. Ensure it's attached for proper integration.");
        }
    }

    /// <summary>
    /// Applies damage to the enemy.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        Debug.Log($"{gameObject.name} took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // Trigger death sequence if health is depleted
        }
    }

    /// <summary>
    /// Handles the enemy's death behavior.
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        // Disable AI behaviors
        if (bigRobotAI != null)
        {
            bigRobotAI.enabled = false;
        }

        // Play death effect if assigned
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Notify the GameManager that an enemy has been defeated
        if (GameManagerScript.Instance != null)
        {
            GameManagerScript.Instance.IncrementEnemiesDefeated();
        }
        else
        {
            Debug.LogWarning("GameManagerScript instance not found. Cannot increment enemies defeated.");
        }

        // Additional death logic (e.g., drop items, play animations) can be added here

        // Destroy the enemy immediately
        Destroy(gameObject);
    }

    /// <summary>
    /// Heals the enemy by a specified amount.
    /// </summary>
    /// <param name="healAmount">Amount to heal.</param>
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed maxHealth
        Debug.Log($"{gameObject.name} healed by {healAmount}. Current Health: {currentHealth}");
    }

    /// <summary>
    /// Retrieves the current health of the enemy.
    /// </summary>
    /// <returns>Current health value.</returns>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Retrieves the maximum health of the enemy.
    /// </summary>
    /// <returns>Maximum health value.</returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}


using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Maximum health points
    private int currentHealth; // Current health points

    [Header("Events")]
    public UnityEvent onDeath; // Event triggered on player death
    public UnityEvent onHealthChanged; // Event triggered when health changes

    private void Start()
    {
        ResetHealth(); // Initialize health at the start of the game
    }

    // Resets the player's health to maximum
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged.Invoke(); // Notify that health has changed
    }

    // Method to apply damage to the player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        onHealthChanged.Invoke(); // Notify that health has changed

        if (currentHealth <= 0)
        {
            Die(); // Trigger death if health is zero or below
        }
    }

    // Method to handle player death
    private void Die()
    {
        onDeath.Invoke(); // Trigger death event
        // Additional game over logic can be implemented here
        // For example, you could display a game over screen or restart the level
        Debug.Log("Player has died. Game Over!");
        ResetHealth(); // Reset health for the next game session (if applicable)
    }

    // Method to get the current health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // Method to get the maximum health
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}


using UnityEngine;

public class PlayerEnhancementManager : MonoBehaviour
{
    // Player's current health and maximum health
    private float currentHealth;
    private float maxHealth;

    // Player's movement speed
    private float movementSpeed;

    // Player's shooting mechanics
    private float reloadTime;
    private float firingRate;

    // Initialize default values
    private void Start()
    {
        maxHealth = 100f; // Default maximum health
        currentHealth = maxHealth; // Start with full health
        movementSpeed = 5f; // Default movement speed
        reloadTime = 1f; // Default reload time
        firingRate = 1f; // Default firing rate
    }

    // Method to increase maximum health
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Restore health to new max
        Debug.Log("Max health increased to: " + maxHealth);
    }

    // Method to enhance movement speed
    public void EnhanceMovementSpeed(float amount)
    {
        movementSpeed += amount;
        Debug.Log("Movement speed increased to: " + movementSpeed);
    }

    // Method to improve reload time
    public void ImproveReloadTime(float amount)
    {
        reloadTime = Mathf.Max(0.1f, reloadTime - amount); // Ensure reload time doesn't go below a threshold
        Debug.Log("Reload time improved to: " + reloadTime);
    }

    // Method to increase firing rate
    public void IncreaseFiringRate(float amount)
    {
        firingRate += amount;
        Debug.Log("Firing rate increased to: " + firingRate);
    }

    // Method to apply enhancements based on player actions
    public void AcquireEnhancement(string enhancementType, float amount)
    {
        switch (enhancementType)
        {
            case "Health":
                IncreaseMaxHealth(amount);
                break;
            case "Speed":
                EnhanceMovementSpeed(amount);
                break;
            case "Reload":
                ImproveReloadTime(amount);
                break;
            case "FireRate":
                IncreaseFiringRate(amount);
                break;
            default:
                Debug.LogWarning("Unknown enhancement type: " + enhancementType);
                break;
        }
    }

    // Method to get current player stats (for debugging or UI display)
    public void DisplayCurrentStats()
    {
        Debug.Log($"Current Health: {currentHealth}/{maxHealth}, Movement Speed: {movementSpeed}, Reload Time: {reloadTime}, Firing Rate: {firingRate}");
    }
}

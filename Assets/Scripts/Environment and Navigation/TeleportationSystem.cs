
using UnityEngine;

public class TeleportationSystem : MonoBehaviour
{
    // Reference to the corresponding teleportation door in another room
    public TeleportationSystem correspondingDoor;

    // Method called when the player enters the teleportation door
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the corresponding door's position
            TeleportPlayer(other.transform);
        }
    }

    // Teleports the player to the corresponding door
    private void TeleportPlayer(Transform playerTransform)
    {
        // Ensure the corresponding door is set
        if (correspondingDoor != null)
        {
            // Move the player to the corresponding door's position
            playerTransform.position = correspondingDoor.transform.position;

            // Optionally, add any teleportation effects or sounds here
            // Example: Play teleport sound or visual effect
            Debug.Log("Player teleported to: " + correspondingDoor.transform.position);
        }
        else
        {
            Debug.LogWarning("Corresponding door not set for teleportation.");
        }
    }
}

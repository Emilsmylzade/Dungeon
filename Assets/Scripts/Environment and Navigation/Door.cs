using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Teleportation Settings")]
    public bool isConnected = false; // Indicates if the door is already connected
    public Door linkedDoor; // Reference to the door this one connects to
    public float teleportOffset = 1.5f; // Distance to offset after teleporting

    [Header("Teleport Points")]
    public Transform teleportPoint; // The designated teleport point for this door

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug log to confirm trigger activation
        Debug.Log($"OnTriggerEnter2D triggered by: {other.name}");

  

        if (other.CompareTag("Player") && linkedDoor != null && linkedDoor.teleportPoint != null)
        {
            Debug.Log("Player detected for teleportation.");

            // Calculate the teleport position
            Vector3 teleportPosition = linkedDoor.teleportPoint.position;

            // Teleport the player
            other.transform.position = teleportPosition;
            Debug.Log($"Player teleported to {teleportPosition}.");

        }
        else
        {
            if (linkedDoor == null)
                Debug.LogWarning("Linked door is not assigned.");
            if (linkedDoor != null && linkedDoor.teleportPoint == null)
                Debug.LogWarning("Linked door's teleportPoint is not assigned.");
        }
    }
}

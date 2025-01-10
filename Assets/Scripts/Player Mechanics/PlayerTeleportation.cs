
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    public float teleportDistance = 5f; // Distance to teleport
    public float teleportCooldown = 2f; // Cooldown time for teleportation
    private float lastTeleportTime; // Time when the last teleport occurred

    public GameObject teleportEffect; // Visual effect for teleportation

    void Update()
    {
        // Check for left mouse button click and if the cooldown has expired
        if (Input.GetMouseButtonDown(0) && Time.time >= lastTeleportTime + teleportCooldown)
        {
            Teleport();
        }
    }

    // Function to handle teleportation
    private void Teleport()
    {
        // Calculate the teleport position based on the player's current position and direction
        Vector3 teleportDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        teleportDirection.z = 0; // Ensure we only teleport in the x-y plane
        Vector3 teleportPosition = transform.position + teleportDirection * teleportDistance;

        // Move the player to the new teleport position
        transform.position = teleportPosition;

        // Trigger teleportation visual effect
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, transform.position, Quaternion.identity);
        }

        // Update the last teleport time
        lastTeleportTime = Time.time;
    }
}

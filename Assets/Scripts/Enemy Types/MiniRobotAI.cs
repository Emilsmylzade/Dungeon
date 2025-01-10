
using UnityEngine;

public class MiniRobotAI : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed at which the MiniRobot moves
    public float attackRange = 5f; // Range within which the MiniRobot can attack the player
    public float safeDistance = 7f; // Distance to maintain from the player
    public float laserCooldown = 1f; // Time between laser attacks
    public GameObject laserPrefab; // Prefab for the laser projectile
    public Transform firePoint; // Point from which the laser is fired
    private Transform player; // Reference to the player's transform
    private float nextAttackTime = 0f; // Timer for the next attack

    void Start()
    {
        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within attack range, attempt to attack
        if (distanceToPlayer < attackRange)
        {
            AttackPlayer();
        }
        else
        {
            // Move away from the player to maintain a safe distance
            MoveAwayFromPlayer();
        }
    }

    void MoveAwayFromPlayer()
    {
        // Calculate direction away from the player
        Vector2 direction = (transform.position - player.position).normalized;

        // Move the MiniRobot in the opposite direction of the player
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Ensure the MiniRobot maintains a minimum distance from the player
        if (Vector2.Distance(transform.position, player.position) < safeDistance)
        {
            // If too close, move further away
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        // Implement erratic movement by adding a slight random offset
        float erraticOffsetX = Random.Range(-0.1f, 0.1f);
        float erraticOffsetY = Random.Range(-0.1f, 0.1f);
        transform.position += new Vector3(erraticOffsetX, erraticOffsetY, 0);
    }

    void AttackPlayer()
    {
        // Check if the cooldown period has passed
        if (Time.time >= nextAttackTime)
        {
            // Instantiate and fire the laser projectile
            Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
            nextAttackTime = Time.time + laserCooldown; // Reset the attack timer
        }
    }
}

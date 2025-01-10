
using System.Collections;
using UnityEngine;

public class BigRobotAI : MonoBehaviour
{
    public float moveSpeed = 3.5f; // Speed at which the BigRobot moves towards the player
    public float attackRange = 1.5f; // Range within which the BigRobot can attack the player
    public float chargeDistance = 5f; // Distance at which the BigRobot will start charging
    public float chargeSpeed = 7f; // Speed during the charge attack
    public float attackCooldown = 1.5f; // Time between attacks
    private float lastAttackTime; // Time when the last attack occurred

    private Transform player; // Reference to the player's transform
    private bool isCharging; // Flag to check if the BigRobot is currently charging

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position); // Calculate distance to player

            if (distanceToPlayer < chargeDistance && !isCharging)
            {
                StartCoroutine(ChargeAtPlayer()); // Start charging if within charge distance
            }
            else if (distanceToPlayer < attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer(); // Attack if within attack range and cooldown is over
            }
            else if (!isCharging)
            {
                MoveTowardsPlayer(distanceToPlayer); // Move towards player if not charging or attacking
            }
        }
    }

    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized; // Get direction to player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); // Move towards player
        }
    }

    private void AttackPlayer()
    {
        lastAttackTime = Time.time; // Update last attack time
        // Implement attack logic here (e.g., deal damage to player)
        Debug.Log("BigRobot attacks the player!"); // Placeholder for attack logic
        // Example damage logic
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); // Deal 10 damage to the player
        }
    }

    private IEnumerator ChargeAtPlayer()
    {
        isCharging = true; // Set charging flag
        Vector2 chargeDirection = (player.position - transform.position).normalized; // Get direction to charge
        float chargeDuration = 0.5f; // Duration of the charge
        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration)
        {
            transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime; // Move in charge direction
            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        isCharging = false; // Reset charging flag after charge is complete
    }
}

using UnityEngine;

public class PencilArrowProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f; // Speed of the projectile
    public int damage = 25; // Damage dealt by the projectile
    public GameObject impactEffect; // Visual effect on impact
    public AudioClip impactSound; // Sound played on impact

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on projectile.");
        }

        // Ensure gravity doesn't affect the projectile
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.rotation = 0f; // Optional: Reset rotation if necessary
        }

        // Optionally, you can handle shooting sound here if desired
        // Uncomment the following lines if you prefer this approach
        /*
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        }
        */
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        // Rotate the projectile to face its movement direction
        RotateProjectile();
    }

    private void RotateProjectile()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f; // Adjust based on sprite orientation
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            HandleImpact();
        }
        else if (collision.CompareTag("Wall"))
        {
            HandleImpact();
        }
    }

    private void HandleImpact()
    {
        // Play impact sound
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }

        // Instantiate impact effect
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Destroy the projectile after impact
        Destroy(gameObject);
    }
}


using UnityEngine;

public class ResumeWeapon : MonoBehaviour
{
    public float throwForce = 10f; // Force applied when the weapon is thrown
    public float explosionRadius = 5f; // Radius of the explosion effect
    public int explosionDamage = 50; // Damage dealt to enemies within the explosion
    public GameObject explosionEffect; // Prefab for the explosion visual effect
    public AudioClip explosionSound; // Sound effect for the explosion

    private bool isThrown = false; // Track if the weapon has been thrown

    // Method to throw the Resume weapon
    public void Throw(Vector2 direction)
    {
        if (!isThrown)
        {
            isThrown = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * throwForce, ForceMode2D.Impulse);
            // Optionally, you can disable the collider to prevent immediate collision
            // GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 5f); // Destroy the weapon after 5 seconds if it doesn't explode
        }
    }

    // Method called when the weapon collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    // Method to handle the explosion logic
    private void Explode()
    {
        // Instantiate explosion effect
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Play explosion sound
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Detect enemies within the explosion radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Assuming enemies have a method to take damage
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(explosionDamage);
            }
        }

        // Destroy the Resume weapon after explosion
        Destroy(gameObject);
    }

    // Optional: Draw the explosion radius in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}


using UnityEngine;

public class PencilArrowProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 25; // Damage dealt by the projectile
    public GameObject impactEffect; // Visual effect on impact
    public AudioClip shootSound; // Sound played when the projectile is fired
    public AudioClip impactSound; // Sound played on impact

    private void Start()
    {
        // Play shooting sound
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
    }

    private void Update()
    {
        // Move the projectile forward at the specified speed
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);

            // Play impact sound
            AudioSource.PlayClipAtPoint(impactSound, transform.position);

            // Instantiate impact effect
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }

            // Destroy the projectile after impact
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            // Play impact sound on wall collision
            AudioSource.PlayClipAtPoint(impactSound, transform.position);

            // Instantiate impact effect
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }

            // Destroy the projectile upon hitting a wall
            Destroy(gameObject);
        }
    }
}

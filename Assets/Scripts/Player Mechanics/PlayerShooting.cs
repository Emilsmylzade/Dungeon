using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab for the projectile
    public AudioClip shootSound; // Sound to play when shooting
    private AudioSource audioSource; // Audio source for playing sounds
    public PlayerMovement playerMovement;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            // If no AudioSource is attached, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check for left mouse button click to shoot
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Calculate the direction to the mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootingPosition = playerMovement.aimTransform.position;
        Vector2 direction = (mousePosition - shootingPosition).normalized;

        // Instantiate the projectile at the shooting point with rotation facing the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Adjust based on sprite orientation
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition, rotation);

        // Set the velocity of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        PencilArrowProjectile paProjectile = projectile.GetComponent<PencilArrowProjectile>();

        if (rb != null && paProjectile != null)
        {
            rb.velocity = direction * paProjectile.speed;
        }
        else
        {
            Debug.LogWarning("Projectile Rigidbody2D or PencilArrowProjectile script missing.");
        }

        // Play shooting sound
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Optional: Add visual feedback (e.g., particle effects) here
        // Example: Instantiate(particleEffectPrefab, shootingPoint.position, Quaternion.identity);
    }
}

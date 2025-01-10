
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab for the projectile
    public Transform shootingPoint; // Point from where the projectile will be instantiated
    public float projectileSpeed = 10f; // Speed of the projectile
    public float damage = 10f; // Damage dealt by the projectile
    public AudioClip shootSound; // Sound to play when shooting
    private AudioSource audioSource; // Audio source for playing sounds

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        // Check for right mouse button click to shoot
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the projectile at the shooting point
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        // Calculate the direction to the mouse position
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootingPoint.position).normalized;
        // Set the projectile's velocity
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        // Play shooting sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Optional: Add visual feedback (e.g., particle effects) here
        // Example: Instantiate a particle effect at the shooting point
        // Instantiate(particleEffectPrefab, shootingPoint.position, Quaternion.identity);
    }
}

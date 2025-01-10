
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player character
    public Transform aimTransform; // Transform to indicate where the player is aiming
    public GameObject weapon1; // First weapon
    public GameObject weapon2; // Second weapon
    private GameObject currentWeapon; // Currently equipped weapon
    private Vector2 movementInput; // Stores movement input

    void Start()
    {
        currentWeapon = weapon1; // Start with the first weapon equipped
        currentWeapon.SetActive(true); // Ensure the current weapon is active
        weapon2.SetActive(false); // Ensure the second weapon is inactive
    }

    void Update()
    {
        HandleMovement(); // Call movement handling
        HandleAiming(); // Call aiming handling
        HandleWeaponSwitch(); // Call weapon switching handling
    }

    // Handles player movement based on input
    private void HandleMovement()
    {
        // Get input from arrow keys or WASD
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        // Move the player character
        Vector2 move = movementInput.normalized * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    // Handles aiming mechanics for twin-stick shooter functionality
    private void HandleAiming()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = (mousePosition - transform.position).normalized;

        // Rotate the aim transform to face the mouse position
        aimTransform.up = aimDirection;
    }

    // Switches between two weapons when the player presses the 'Q' key
    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    // Switches the current weapon
    private void SwitchWeapon()
    {
        if (currentWeapon == weapon1)
        {
            currentWeapon.SetActive(false); // Deactivate current weapon
            currentWeapon = weapon2; // Switch to the second weapon
        }
        else
        {
            currentWeapon.SetActive(false); // Deactivate current weapon
            currentWeapon = weapon1; // Switch back to the first weapon
        }
        currentWeapon.SetActive(true); // Activate the new current weapon
    }
}

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
        // Ensure there's a main camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please tag your camera as 'MainCamera'.");
            return;
        }

        // Get the mouse position in world space (2D)
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the player to the mouse position
        Vector2 direction = (mouseWorldPosition - (Vector2)transform.position).normalized;

        // Define the maximum distance the aimTransform can be from the player
        float aimDistance = 0.5f; // Adjust this value as needed

        // Calculate the new position for aimTransform
        Vector2 newAimPosition = (Vector2)transform.position + direction * aimDistance;

        // Update the aimTransform's position
        aimTransform.position = new Vector3(newAimPosition.x, newAimPosition.y, aimTransform.position.z);

        // Optional: Visualize the aiming direction in the Scene view
        Debug.DrawLine(transform.position, mouseWorldPosition, Color.red);
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
            weapon1.SetActive(false); // Deactivate first weapon
            currentWeapon = weapon2; // Switch to the second weapon
            weapon2.SetActive(true); // Activate second weapon
        }
        else
        {
            weapon2.SetActive(false); // Deactivate second weapon
            currentWeapon = weapon1; // Switch back to the first weapon
            weapon1.SetActive(true); // Activate first weapon
        }
    }
}

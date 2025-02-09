using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;  // Mouse sensitivity
    public bool invertY = false;           // Option to invert Y-axis

    [Header("Clamping")]
    public float minPitch = -90f;          // Minimum vertical rotation
    public float maxPitch = 90f;           // Maximum vertical rotation

    [Header("Smooth Movement")]
    public float smoothTime = 0.1f;        // Smooth time for rotation

    [Header("References")]
    public Transform playerBody;           // Reference to the player's body for horizontal rotation

    private float pitch = 0f;              // Vertical rotation (up/down)
    private Vector2 currentMouseDelta;     // Smoothed mouse delta
    private Vector2 currentMouseDeltaVelocity; // Velocity for smooth damp

    private bool isPaused = false;         // Tracks if the game is paused

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the screen
        Cursor.visible = false;                  // Hide the cursor
    }

    void Update()
    {
        // Handle pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Only process camera movement when not paused
        if (!isPaused && Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        // Get raw mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Invert Y-axis if enabled
        mouseY = invertY ? -mouseY : mouseY;

        // Smooth the mouse input
        Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, smoothTime);

        // Rotate the camera up/down (pitch)
        pitch -= currentMouseDelta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // Clamp the pitch to prevent over-rotation
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Rotate the player body left/right (yaw)
        playerBody.Rotate(Vector3.up * currentMouseDelta.x);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true;                 // Show the cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false;                  // Hide the cursor
        }
    }

    // Optional: Expose sensitivity adjustment for runtime changes
    public void SetMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }

    // Optional: Expose Y-axis inversion toggle for runtime changes
    public void SetInvertY(bool invert)
    {
        invertY = invert;
    }
}

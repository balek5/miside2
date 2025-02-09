using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprintInput;

    [Header("Camera Settings")]
    public Transform playerCamera;      // Reference to the camera transform
    public Transform playerBody;        // Reference to the player body transform
    public float mouseSensitivity = 100f; // Sensitivity for mouse movement
    private float pitch = 0f;           // Vertical camera rotation

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // Movement input from WASD/Arrow keys
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            // Camera input from mouse
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // Sprint input
            playerControls.PlayerActions.W.performed += i => sprintInput = true;
            playerControls.PlayerActions.W.canceled += i => sprintInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleSprintingInput();
    }

    private void HandleMovementInput()
    {
        // Movement input (WASD or Arrow keys)
        verticalInput = movementInput.y;  // Forward/backward
        horizontalInput = movementInput.x;  // Left/right

        // Calculate move amount
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        // Update animator values (if using an animator)
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleCameraInput()
    {
        // Get mouse input
        cameraInputX = cameraInput.x * mouseSensitivity * Time.deltaTime; // Horizontal mouse movement
        cameraInputY = cameraInput.y * mouseSensitivity * Time.deltaTime; // Vertical mouse movement

        // Rotate the camera up/down (pitch)
        pitch -= cameraInputY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Clamp the pitch to prevent over-rotation

        // Apply vertical rotation to the camera
        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Rotate the player body left/right (yaw)
        playerBody.Rotate(Vector3.up * cameraInputX);
    }

    private void HandleSprintingInput()
    {
        // Sprinting logic
        if (sprintInput && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }
}

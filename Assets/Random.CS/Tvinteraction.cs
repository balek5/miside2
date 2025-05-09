using UnityEngine;

public class TVInteract : MonoBehaviour
{
    public GameObject miniGameCanvas; // Reference to the mini-game canvas
    public Camera playerCamera; // The camera showing the player
    public Camera gameCamera;   // The camera showing the mini-game
    private bool isNearTV = false; // To track if the player is near the TV
    private bool isMiniGameActive = false; // To track if the mini-game is active

    void Start()
    {
        // Ensure the player camera is active and the game camera is inactive at the start
        playerCamera.gameObject.SetActive(true);
        gameCamera.gameObject.SetActive(false);
        miniGameCanvas.SetActive(false); // Ensure the mini-game UI is inactive at the start

        // Lock the cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isMiniGameActive)
        {
            // Exit the mini-game when the Escape key is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndMiniGame();
            }
        }
        else
        {
            // Check for player input to start the mini-game with the Q key
            if (isNearTV && Input.GetKeyDown(KeyCode.Q)) // 'Q' key to interact
            {
                StartMiniGame();
            }
        }
    }

    private void StartMiniGame()
    {
        isMiniGameActive = true;

        // Unlock the cursor when switching to the mini-game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Switch to the mini-game view
        miniGameCanvas.SetActive(true); // Activate the mini-game UI
        playerCamera.gameObject.SetActive(false); // Deactivate the player camera
        gameCamera.gameObject.SetActive(true); // Activate the mini-game camera
    }

    private void EndMiniGame()
    {
        isMiniGameActive = false;

        // Lock the cursor when exiting the mini-game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Switch back to the main game
        miniGameCanvas.SetActive(false); // Deactivate the mini-game UI
        playerCamera.gameObject.SetActive(true); // Reactivate the player camera
        gameCamera.gameObject.SetActive(false); // Deactivate the mini-game camera
    }

    // Detect when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTV = true;
            Debug.Log("Press 'Q' to interact with the TV");
        }
    }

    // Detect when the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTV = false;
            Debug.Log("You are too far from the TV");
        }
    }
}

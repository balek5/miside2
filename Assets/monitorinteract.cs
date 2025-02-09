using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // Added this to support TMP (TextMeshPro) if you're using it

public class MonitorInteraction : MonoBehaviour
{
    public string sceneToLoad = "SceneName"; // Name of the scene you want to load
    public Camera playerCamera; // Reference to the player camera
    public GameObject interactiveCanvas; // Reference to the interactive canvas
    public GameObject loadingCanvas; // Reference to the loading canvas (with loading animation)
    public float interactionDistance = 5f; // Distance at which the player can interact with the monitor
    public LayerMask interactableLayer; // Layer mask to specify which objects can be interacted with
    public Text loadingText; // Reference to the loading text (if using TextMeshPro)

    // Added references for password input field
    public InputField passwordInputField; // Reference to TMP Input Field for password
    public string correctPassword = "12345"; // The correct password
    public Text feedbackText; // Feedback message for incorrect password

    private bool isNearPC = false; // Flag to track if the player is near the PC

    void Start()
    {
        // Ensure the interactive canvas and loading canvas are hidden at the start
        if (interactiveCanvas != null)
        {
            interactiveCanvas.SetActive(false); // Hide the interactive canvas initially
        }
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false); // Hide the loading canvas initially
        }

        // Make sure necessary references are assigned
        if (playerCamera == null)
            Debug.LogError("Player Camera reference is missing!");
        if (interactiveCanvas == null)
            Debug.LogError("Interactive Canvas reference is missing!");
        if (loadingCanvas == null)
            Debug.LogError("Loading Canvas reference is missing!");
        if (passwordInputField == null)
            Debug.LogError("Password Input Field reference is missing!");
        if (loadingText == null)
            Debug.LogError("Loading Text reference is missing!");
        if (feedbackText == null)
            Debug.LogError("Feedback Text reference is missing!");
    }

    void Update()
    {
        // Perform a raycast from the camera to check if the player is looking at the monitor
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            if (hit.collider.CompareTag("PC"))
            {
                if (!isNearPC)
                {
                    Debug.Log("Player is near the PC. Press 'F' to interact.");
                    isNearPC = true;
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("F key pressed. Showing the interactive canvas...");
                    ShowInteractiveCanvas();
                }
            }
        }
        else
        {
            if (isNearPC)
            {
                Debug.Log("Player is no longer near the PC.");
                isNearPC = false;
            }
        }
    }

    private void ShowInteractiveCanvas()
    {
        if (interactiveCanvas != null)
            interactiveCanvas.SetActive(true);
    }

    public void OnSubmitPassword()
    {
        if (passwordInputField.text.Trim() == correctPassword)
        {
            Debug.Log("Password correct. Starting loading animation...");
            feedbackText.text = "Access Granted";
            ShowLoadingAnimation();
            StartCoroutine(LoadSceneAfterDelay(2f));
        }
        else
        {
            Debug.Log("Incorrect password.");
            feedbackText.text = "Incorrect password. Try again.";
        }
    }

    private void ShowLoadingAnimation()
    {
        if (loadingCanvas != null)
            loadingCanvas.SetActive(true);

        if (loadingText != null)
            loadingText.text = "PC Starting...";
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene name is empty or null!");
            return;
        }

        if (!SceneIsInBuildSettings(sceneToLoad))
        {
            Debug.LogError("Scene " + sceneToLoad + " is not added to Build Settings!");
            return;
        }

        Debug.Log("Loading scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    private bool SceneIsInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneManager.GetSceneByBuildIndex(i).name == sceneName)
                return true;
        }
        return false;
    }
}
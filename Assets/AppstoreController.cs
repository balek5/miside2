using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AppStoreController : MonoBehaviour
{
    public GameObject appStoreUI; // Reference to the app store UI (the parent panel)
    public Button downloadButton; // Button for downloading the game
    public GameObject gameDownloadPanel; // Panel to show when the game is being downloaded
    public Button launchGameButton; // Button to launch the game after downloading

    private bool isGameDownloaded = false;

    void Start()
    {
        // Hide the download panel and game launch button initially
        gameDownloadPanel.SetActive(false);
        launchGameButton.gameObject.SetActive(false);

        // Add listeners to buttons
        downloadButton.onClick.AddListener(OnDownloadGame);
        launchGameButton.onClick.AddListener(OnLaunchGame);
    }

    // Handle downloading the game
    private void OnDownloadGame()
    {
        // Show the download panel and simulate downloading
        gameDownloadPanel.SetActive(true);
        downloadButton.interactable = false; // Disable the download button while downloading

        // Simulate download completion after a delay (e.g., 3 seconds)
        Invoke("CompleteDownload", 3f);
    }

    // Complete the download and enable the launch button
    private void CompleteDownload()
    {
        isGameDownloaded = true;
        gameDownloadPanel.SetActive(false); // Hide the download panel
        launchGameButton.gameObject.SetActive(true); // Show the launch button
    }

    // Launch the game (teleport to the next scene)
    private void OnLaunchGame()
    {
        if (isGameDownloaded)
        {
            // Load the next scene (e.g., the game world)
            SceneManager.LoadScene("NextWorldScene");
        }
    }

    // Close the app store
    public void CloseAppStore()
    {
        appStoreUI.SetActive(false); // Hide the app store
    }
}
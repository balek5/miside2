using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DownloadConfirmManager : MonoBehaviour
{
    public GameDetailsManager gameDetailsManager;
    [Header("Download Confirmation Popup")]
    public GameObject confirmPopup;        // The popup panel (assign in Inspector)
    public Button yesButton;               // Yes button on the popup
    public Button noButton;                // No button on the popup
    public TMP_Text statusText;            // Optional: Display status messages
     public TMP_Text confirmText;
    void Start()
    {
        // Ensure that the popup is hidden at start.
        confirmPopup.SetActive(false);
        
        // Attach button listeners.
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    // Activates the confirmation popup.
    public void ShowConfirmation()
    {
        confirmPopup.SetActive(true);
        if (statusText != null)
            statusText.text = "";  // Clear any previous messages.
        confirmText.text = "Are you sure you want to play this game?";
    }

    void OnYesClicked()
    {
        confirmPopup.SetActive(false); // Hide the confirmation popup

        if (gameDetailsManager != null)
        {
            gameDetailsManager.gameDetailPanel.SetActive(true); // Reopen the detail panel
            gameDetailsManager.StartDownloadSimulation();       // Start the download animation
        }

        if (statusText != null)
        {
            statusText.text = ""; // Optional: clear status
        }
    }

    void OnNoClicked()
    {
        Debug.Log("Download canceled. Shutting down game...");

        if (statusText != null)
        {
            statusText.text = "Download canceled. Shutting down...";
        }

        confirmPopup.SetActive(false);

        // Exit the game
        Application.Quit();

        // Just in case you're testing in the Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
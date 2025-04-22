using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameDetailsManager : MonoBehaviour
{
    public GameDetailsManager gameDetailsManager; // Reference set in Inspector

    [Header("Game Detail Panel References")]
    public GameObject gameDetailPanel;      // The panel that displays game details (assign in Inspector)
    public Image gameArtwork;               // Image component for the game artwork
    public TMP_Text gameTitleText;          // TMP_Text component for game title
    public TMP_Text gameDescriptionText;    // TMP_Text component for game description
    public Button downloadButton;           // Button to trigger download confirmation

    [Header("Working Game Data")]
    public Sprite workingGameSprite;        // The artwork for the working game
    public string workingGameTitle = "";
    public string workingGameDescription = "";

    [Header("Managers")]
    public DownloadConfirmManager confirmManager;  // Reference to our confirmation manager

    [Header("Other UI Elements")]
    public GameObject iconsPanel;  // Reference to the panel that holds the game icons

    [Header("Download UI")]
    public TMP_Text downloadingText;
    public Slider downloadProgressBar;
    public Button playButton;
    
    void Start()
    {
        // Hide the game detail panel at startup.
        gameDetailPanel.SetActive(false);
        
        // Attach the click event for the download button.
        downloadButton.onClick.AddListener(OnDownloadButtonClicked);
    }

    // Call this method when the working game icon is clicked.
    public void OpenWorkingGameDetails()
    {
        // Optionally hide the game icons if you don't want them visible.
        if (iconsPanel != null)
        {
            iconsPanel.SetActive(false);
        }

        // Fill in the game details.
        gameArtwork.sprite = workingGameSprite;
        gameTitleText.text = workingGameTitle;
        gameDescriptionText.text = workingGameDescription;
        
        // Show the game detail panel.
        gameDetailPanel.SetActive(true);
    }

    void OnDownloadButtonClicked()
    {
        // When the download button is pressed, call the confirmation popup.
        if (confirmManager != null)
        {
            confirmManager.ShowConfirmation();
        }
    }
    
    public void StartDownloadSimulation()
    {
        downloadButton.gameObject.SetActive(false);
        downloadingText.gameObject.SetActive(true);
        downloadProgressBar.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        downloadProgressBar.value = 0;

        StartCoroutine(DownloadRoutine());
    }

    IEnumerator DownloadRoutine()
    {
        float duration = 3f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            downloadProgressBar.value = Mathf.Clamp01(timer / duration);
            yield return null;
        }

        // After download is done
        downloadingText.gameObject.SetActive(false);
        downloadProgressBar.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }
}

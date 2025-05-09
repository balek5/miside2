using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this line
using System.Collections;

public class AppStoreManager : MonoBehaviour
{
    // App Store UI References
    public GameObject appStoreWindow;  // The whole app store canvas or window
    public Button openAppStoreButton;
    public Button closeAppStoreButton;
    public Button downloadButton;
    public Button launchButton;
    public Image ImageFill;
    public Text downloadText;
    public GameObject downloadPanel;

    // Game Info Panel UI References
    public Button gameNameButton;
    public GameObject gameInfoPanel;
    public Image additionalImage;

    // Confirmation Popup UI References
    public GameObject confirmationPopup;
    public Button yesButton;
    public Button noButton;
    public Text warningText;
    public Image popupImage;  // Reference to the background image of the popup for glitchy textures

    // Download Process Flags and Variables
    private bool isDownloading = false;
    private float downloadProgress = 0f;

    // Flag to stop the glitch effect
    private bool isGlitching = false;

    public void Start()
    {
        // Set initial UI states
        appStoreWindow.SetActive(false);
        downloadPanel.SetActive(false);
        launchButton.gameObject.SetActive(false);
        ImageFill.gameObject.SetActive(false);
        downloadText.gameObject.SetActive(false);

        additionalImage.gameObject.SetActive(false);
        gameInfoPanel.SetActive(false);

        openAppStoreButton.onClick.AddListener(OpenAppStore);
        closeAppStoreButton.onClick.AddListener(CloseAppStore);
        downloadButton.onClick.AddListener(StartDownload);
        launchButton.onClick.AddListener(OpenConfirmationPopup);

        gameNameButton.onClick.AddListener(OpenGameInfoPanel);

        yesButton.onClick.AddListener(LaunchGame);
        noButton.onClick.AddListener(CloseConfirmationPopup);

        warningText.gameObject.SetActive(false);
    }

    // Opens the app store window
    public void OpenAppStore()
    {
        appStoreWindow.SetActive(true);
    }

    // Closes the app store window
    public void CloseAppStore()
    {
        appStoreWindow.SetActive(false);
    }

    // Opens the Game Info Panel when the game name button is clicked
    public void OpenGameInfoPanel()
    {
        gameNameButton.gameObject.SetActive(false);
        gameInfoPanel.SetActive(true);
        additionalImage.gameObject.SetActive(true);
    }

    // Starts the download process
    public void StartDownload()
    {
        if (!isDownloading)
        {
            isDownloading = true;
            downloadButton.interactable = false;
            downloadText.gameObject.SetActive(true);
            downloadText.text = "Downloading...";
            ImageFill.gameObject.SetActive(true);
            StartCoroutine(DownloadGame());
        }
    }

    // Coroutine to simulate the download process
    IEnumerator DownloadGame()
    {
        while (downloadProgress < 1f)
        {
            downloadProgress += 0.05f;
            ImageFill.fillAmount = downloadProgress;
            yield return new WaitForSeconds(0.1f);
        }

        downloadText.text = "Download Complete!";
        downloadButton.gameObject.SetActive(false);
        launchButton.gameObject.SetActive(true);
    }

    // Open the confirmation popup with glitchy effects
    public void OpenConfirmationPopup()
    {
        confirmationPopup.SetActive(true);
        warningText.gameObject.SetActive(true);

        // Start glitching effect
        isGlitching = true;
        StartCoroutine(GlitchEffect());
    }

    // Close the confirmation popup
    public void CloseConfirmationPopup()
    {
        confirmationPopup.SetActive(false);
        warningText.gameObject.SetActive(false);
    }
    public void LaunchGame()
    {
        Debug.Log("Yes Button Clicked - Attempting to Load SampleScene...");

        isGlitching = false; // Stop glitching effect
        CloseConfirmationPopup();
    
        // Attempt to load the scene
        try
        {
            SceneManager.LoadScene("SampleScene"); 
        }
        catch (System.Exception e)
        {
            Debug.LogError("Scene load failed: " + e.Message);
        }
    }

    // Glitch effect coroutine (intense and continuous until 'Yes' button is pressed)
    private IEnumerator GlitchEffect()
    {
        while (isGlitching)
        {
            // Apply glitching to the entire app store window (canvas)
            ApplyCanvasGlitch(appStoreWindow);

            // Apply random jitter to the app store window
            appStoreWindow.transform.position = new Vector3(
                appStoreWindow.transform.position.x + Random.Range(-2f, 2f),
                appStoreWindow.transform.position.y + Random.Range(-2f, 2f),
                appStoreWindow.transform.position.z
            );

            // Randomize the color of the entire canvas for a glitch effect
            appStoreWindow.GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value);

            // Apply random scale to the popup for distortion
            float scaleAmount = Random.Range(0.9f, 1.1f);
            confirmationPopup.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1f);

            // Optionally, apply a glitch texture flicker effect
            if (popupImage.material != null)
            {
                popupImage.material.SetFloat("_GlitchAmount", Random.Range(0f, 1f)); // Example of a glitch shader effect
            }

            // Apply random size shifting to the app store window (canvas)
            ApplyRandomSizeShift(appStoreWindow);

            // Add some random flickering
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f)); // Random time for glitch flicker
        }

        // Reset the popup and text to their original state after the glitch ends
        confirmationPopup.transform.localScale = Vector3.one;
        warningText.transform.position = new Vector3(0, 0, 0);
        warningText.color = Color.white;
    }

    // Apply glitching to the entire app store window (canvas)
    private void ApplyCanvasGlitch(GameObject canvas)
    {
        // Randomly shift the canvas
        float shiftAmountX = Random.Range(-5f, 5f);
        float shiftAmountY = Random.Range(-5f, 5f);
        canvas.transform.position = new Vector3(
            canvas.transform.position.x + shiftAmountX,
            canvas.transform.position.y + shiftAmountY,
            canvas.transform.position.z
        );

        // Randomly scale the canvas for a distortion effect
        float scaleAmount = Random.Range(0.95f, 1.05f);
        canvas.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1f);
    }

    // Apply random size shifting to the entire app store window (canvas)
    private void ApplyRandomSizeShift(GameObject canvas)
    {
        // Randomly change the size of the canvas to simulate size shifting
        float randomScaleX = Random.Range(0.9f, 1.2f); // Random scale between 0.9 and 1.2
        float randomScaleY = Random.Range(0.9f, 1.2f); // Random scale between 0.9 and 1.2

        // Apply the new scale to the canvas
        canvas.transform.localScale = new Vector3(randomScaleX, randomScaleY, 1f);
    }

    // Tweak button with random position and scale changes

}
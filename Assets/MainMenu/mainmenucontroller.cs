using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuCanvas;  // Reference to the main menu canvas
    [SerializeField] private GameObject settingsCanvas;  // Reference to the settings canvas
    [SerializeField] private GameObject creditsPanel;  // Panel for credits (if applicable)

    [Header("Loading Screen")]
    public GameObject loadingScreenPrefab;
    private GameObject loadingScreenCanvas;
    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public Slider loadingSlider;
    public float loadingTime = 5f; // Simulated load time
    public string sceneToLoad = "GameScene"; // Set this in the Inspector

    [Header("Game State")]
    public bool hasSavedGame = false;

    private void Start()
    {
        // Ensure the main menu and credits panels are hidden or active at the start
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        if (settingsCanvas != null) settingsCanvas.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        // Instantiate the loading screen if it's provided in the Inspector
        if (loadingScreenPrefab != null)
        {
            loadingScreenCanvas = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreenCanvas);
            loadingScreen = loadingScreenCanvas.transform.Find("LoadingScreen").gameObject;
            loadingScreen.SetActive(false);
        }
    }

    // === Open and Close Panels ===
    public void OpenSettings()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);  // Disable main menu canvas
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // Enable settings canvas
    }

    public void CloseSettings()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);   // Enable main menu canvas
        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // Disable settings canvas
    }

    // === Open and Close Credits ===
    public void OpenCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    // === Start New Game ===
    public void StartNewGame()
    {
        StartCoroutine(LoadGameWithFixedTime()); // Simulate loading and then load the scene
    }

    // === Continue Game ===
    public void ContinueGame()
    {
        if (hasSavedGame)
        {
            StartCoroutine(LoadGameWithFixedTime());
        }
        else
        {
            Debug.Log("No saved game to continue.");
        }
    }

    // === Load Game from Save ===
    public void LoadGame()
    {
        Debug.Log("Load game pressed");
    }

    // === Start Game Coroutine with Loading Screen ===
    private IEnumerator LoadGameWithFixedTime()
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);  // Show loading screen

        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            float progress = Mathf.Clamp01(elapsedTime / loadingTime);

            if (loadingSlider != null)
                loadingSlider.value = progress;

            if (loadingText != null)
                loadingText.text = "Loading... " + Mathf.FloorToInt(progress * 100) + "%";

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (loadingText != null)
            loadingText.text = "Loading Complete!";

        yield return new WaitForSeconds(1f); // Wait for a second before loading the scene

        SceneManager.LoadScene(sceneToLoad); // Load the specified scene
    }

    // === Quit the Game ===
    public void QuitGame()
    {
        Debug.Log("Quit Game pressed");
        Application.Quit();
    }

    private void Update()
    {
        // You can add menu input logic here if needed
    }
}

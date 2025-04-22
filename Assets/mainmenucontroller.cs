using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gameSettingsPanel;

    [Header("Loading Screen")]
    public GameObject loadingScreenPrefab; // Drag your loading screen prefab here
    private GameObject loadingScreenCanvas; // Instantiate this in code
    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public Slider loadingSlider;
    public float loadingTime = 5f; // Simulated load time
    public string sceneToLoad = "Wakingupscene"; // Set this in the Inspector

    private void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (gameSettingsPanel != null) gameSettingsPanel.SetActive(false);

        // Instantiate the loading screen prefab and make it persist
        if (loadingScreenPrefab != null)
        {
            loadingScreenCanvas = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreenCanvas);
            loadingScreen = loadingScreenCanvas.transform.Find("LoadingScreen").gameObject; // Assuming it's named "LoadingScreen" inside the prefab
            loadingScreen.SetActive(false); // Hide it initially
        }
    }

    // === Settings Panels ===
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void OpenGameSettings()
    {
        if (gameSettingsPanel != null) gameSettingsPanel.SetActive(true);
        Debug.Log("Game Settings panel opened");
    }

    public void CloseGameSettings()
    {
        if (gameSettingsPanel != null) gameSettingsPanel.SetActive(false);
        Debug.Log("Game Settings panel closed");
    }

    // === Start Game with Loading Screen ===
    public void StartGame()
    {
        StartCoroutine(LoadGameWithFixedTime());
    }

    private IEnumerator LoadGameWithFixedTime()
    {
        if (loadingScreen != null) loadingScreen.SetActive(true); // Show the loading screen

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

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneToLoad); // Load your next scene
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

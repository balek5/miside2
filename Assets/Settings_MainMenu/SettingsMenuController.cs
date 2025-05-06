using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Audio Settings")] 
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle muteToggle;

    [Header("Display Settings")] 
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Toggle vSyncToggle;

    [Header("Graphics Settings")] 
    public TMP_Dropdown qualityDropdown;
    public Toggle postProcessingToggle;

    [Header("Language Settings")] 
    public TMP_Dropdown languageDropdown;

    [Header("Control Settings")] 
    public Button keyBindingsButton;
    public Slider mouseSensitivitySlider;

    [Header("Accessibility Settings")] 
    public Toggle subtitlesToggle;
    public Toggle colorblindModeToggle;
    public Slider uiScaleSlider;

    [Header("Other")] 
    public Button backButton;

    [Header("Main Menu Reference")] 
    public GameObject mainMenuCanvas; // Reference to Main Menu Canvas
    public GameObject settingsPanel;  // Reference to the Settings Panel

    private Resolution[] resolutions;

    void Start()
    {
        // Populate resolution options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add listeners to UI elements
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        muteToggle.onValueChanged.AddListener(SetMute);

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        vSyncToggle.onValueChanged.AddListener(SetVSync);

        qualityDropdown.onValueChanged.AddListener(SetQuality);
        postProcessingToggle.onValueChanged.AddListener(SetPostProcessing);

        languageDropdown.onValueChanged.AddListener(SetLanguage);

        keyBindingsButton.onClick.AddListener(OpenKeyBindings);
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);

        subtitlesToggle.onValueChanged.AddListener(SetSubtitles);
        colorblindModeToggle.onValueChanged.AddListener(SetColorblindMode);
        uiScaleSlider.onValueChanged.AddListener(SetUIScale);

        backButton.onClick.AddListener(CloseSettings);
    }

    // Audio Settings Methods
    void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    void SetMusicVolume(float value)
    {
        // Implement music volume adjustment
    }

    void SetSFXVolume(float value)
    {
        // Implement SFX volume adjustment
    }

    void SetMute(bool isMuted)
    {
        AudioListener.pause = isMuted;
    }

    // Display Settings Methods
    void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
    }

    // Graphics Settings Methods
    void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    void SetPostProcessing(bool isEnabled)
    {
        // Implement post-processing toggle
    }

    // Language Settings Method
    void SetLanguage(int index)
    {
        // Implement language change
    }

    // Control Settings Methods
    void OpenKeyBindings()
    {
        // Implement key bindings UI
    }

    void SetMouseSensitivity(float value)
    {
        // Implement mouse sensitivity adjustment
    }

    // Accessibility Settings Methods
    void SetSubtitles(bool isEnabled)
    {
        // Implement subtitles toggle
    }

    void SetColorblindMode(bool isEnabled)
    {
        // Implement colorblind mode
    }

    void SetUIScale(float value)
    {
        // Implement UI scale adjustment
    }

    // Other Methods
    public void OpenSettings()
    {
        // Hide Main Menu and show Settings Panel
        Debug.Log("Opening Settings...");
        mainMenuCanvas.SetActive(false);  // Hide the Main Menu Canvas
        settingsPanel.SetActive(true);  // Show the Settings Panel
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings method called!");

        // Show Main Menu and hide Settings Panel
        if (mainMenuCanvas != null)
        {
            Debug.Log("Main menu canvas found, activating...");
            mainMenuCanvas.SetActive(true);  // Show the Main Menu Canvas
        }
        else
        {
            Debug.LogError("Main menu canvas reference is null!");
        }

        if (settingsPanel != null)
        {
            Debug.Log("Deactivating settings panel...");
            settingsPanel.SetActive(false);  // Hide the Settings Panel
        }
        else
        {
            Debug.LogError("Settings panel reference is null!");
        }

        Debug.Log("Settings panel deactivated");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayButtonScript : MonoBehaviour
{
    public GameObject loadingScreenCanvas;   // The canvas for loading screen (activates the loading screen)
    public GameObject loadingScreen;         // The loading screen (panel) with text and slider
    public TMP_Text loadingText;             // Text to show loading message
    public Slider loadingSlider;             // Slider to show progress
    public float loadingTime = 15f;          // Total time to simulate loading (in seconds)

    private void Start()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false); // Hide loading screen at start
        }
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked; showing loading screen.");
    
        // Ensure the loading screen is hidden in the editor but active at runtime
        loadingScreenCanvas.SetActive(true);  // Show the loading screen canvas
        
        // Start the loading process with a fixed delay
        StartCoroutine(LoadGameWithFixedTime());
    }

    private IEnumerator LoadGameWithFixedTime()
    {
        // Show the loading screen panel
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Simulate loading over a fixed time period (e.g., 10-15 seconds)
        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            // Calculate progress as the ratio of elapsed time to total loading time
            float progress = Mathf.Clamp01(elapsedTime / loadingTime);

            // Update the progress slider
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            // Update the loading text
            if (loadingText != null)
            {
                loadingText.text = "Loading... " + Mathf.FloorToInt(progress * 100) + "%";
            }

            elapsedTime += Time.deltaTime;  // Increment elapsed time by time between frames
            yield return null;
        }

        // Once loading time is finished, proceed to the game scene
        loadingText.text = "Loading Complete!";
        
        // You can choose to display the loading screen for a few seconds if you want
        yield return new WaitForSeconds(1f);

        // Now, load the game scene
        SceneManager.LoadScene("IntroScene"); // Replace with the name of your game scene
    }
}

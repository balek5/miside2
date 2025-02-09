using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes
using UnityEngine.UI; // For Button components

public class MainMenuController : MonoBehaviour
{
    // Function to start the game
    public void StartGame()
    {
        try
        {
            Debug.Log("Attempting to load scene...");
            SceneManager.LoadScene("wakingupscene");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("wakingupscene");
        }
    }


    // Function to quit the game
    public void QuitGame()
    {
        // Quit the game
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
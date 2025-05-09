using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneTransition : MonoBehaviour
{
    // This method is public, and it should be visible in the Unity Editor
    public void LoadNextScene()
    {
        // Log to make sure the method is being called
        Debug.Log("Loading next scene...");

        // Ensure the scene name matches the one in the Build Settings
        SceneManager.LoadScene("DesktopScene"); // Replace "NextScene" with the actual name of your scene
    }

    // Optional: A method to make sure the script is properly attached
    private void Start()
    {
        Debug.Log("SceneTransition script attached and ready.");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    public string sceneToLoad = "DesktopScene"; // Name of the scene to load
    public float delay = 5f; // Delay in seconds before loading the next scene

    private void Start()
    {
        // Start the delayed scene loading
        Invoke("LoadNextScene", delay);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
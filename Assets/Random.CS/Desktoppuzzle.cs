using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName = "Desktopscene"; // Set the name of the scene to load

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}

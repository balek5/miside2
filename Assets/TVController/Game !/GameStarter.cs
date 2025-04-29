using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public string sceneNameToLoad = "DanceOffGame"; // Set your scene name in the Inspector

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
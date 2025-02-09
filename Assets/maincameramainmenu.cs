using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene (replace with your actual scene name)
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }

    public void OpenOptions()
    {
        // Open options menu (you can create a new scene or show UI)
        Debug.Log("Options opened");
    }
}
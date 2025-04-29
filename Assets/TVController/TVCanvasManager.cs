// TVCanvasManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class TVCanvasManager : MonoBehaviour
{
    public GameObject tvCanvas;

    public void PlayGame1()
    {
        // Load your first mini-game scene
        SceneManager.LoadScene("Game1Scene");
    }

    public void PlayGame2()
    {
        // Load your second mini-game scene
        SceneManager.LoadScene("Game2Scene");
    }

    public void CloseTV()
    {
        tvCanvas.SetActive(false);
    }
}
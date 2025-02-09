using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public void OnButtonClick()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
}
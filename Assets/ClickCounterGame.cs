using UnityEngine;
using UnityEngine.UI;

public class ClickCounterGame : MonoBehaviour
{
    public Text scoreText; // Reference to the score text UI element
    public Button clickButton; // Reference to the button

    private int score = 0; // Initial score

    void Start()
    {
        // Initialize score text with the starting score
        scoreText.text = "Score: " + score;

        // Add a listener to the button to call IncrementScore when clicked
        clickButton.onClick.AddListener(IncrementScore); // This should be called only once
    }

    public void IncrementScore()
    {
        score++; // Increase score by 1
        scoreText.text = "Score: " + score; // Update the score text
    }
}
using UnityEngine;
using UnityEngine.Playables; // Required for Timeline integration

public class EnemyController : MonoBehaviour
{
   // Define the target position
    public float moveSpeed = 2.0f; // Define the move speed
    private bool isMoving = false;

    // Called when the Timeline signal is triggered
    public void OnEnemySpeaks()
    {
        // Start dialogue
        DialogueManager.Instance.StartDialogue(
            new string[] { "Oh, you're finally awake." },
            new string[] { "Who are you?", "Where am I?", "What do you want?" },
            new bool[] { true, true, true },  // Added missing parameter
            new string[] { "I'm your worst nightmare.", "You're in my domain.", "I want your cooperation." },
            HandleChoiceSelection
        );



        // Allow enemy to move again (if needed)
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            // Move the enemy (example logic
        }
    }

    public void StartDialogue()
    {
        Debug.Log("Enemy dialogue started.");
        // Add your dialogue logic here
    }

    private void HandleChoiceSelection(int choiceIndex)
    {
        if (choiceIndex == -1)
        {
            Debug.Log("Ready for another selection...");
        }
        else if (choiceIndex >= 0)
        {
            Debug.Log("Enemy responds to choice: " + choiceIndex);
        }
    }
}
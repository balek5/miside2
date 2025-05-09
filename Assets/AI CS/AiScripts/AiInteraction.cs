using UnityEngine;
using UnityEngine.UI;

public class AIInteraction : MonoBehaviour
{
    public string[] dialogueLines; // Add dialogue lines in the Inspector
    public float interactionDistance = 3f; // Distance to interact
    public Transform player; // Assign the player transform in the Inspector
    public Text dialogueText; // Assign the UI Text in the Inspector

    private int currentLineIndex = 0;
    private bool isPlayerNearby = false;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            isPlayerNearby = true;
            dialogueText.text = "Press F to talk"; // Show prompt to talk

            if (Input.GetKeyDown(KeyCode.F))
            {
                ShowNextDialogue();
            }
        }
        else
        {
            isPlayerNearby = false;
            dialogueText.text = ""; // Hide text when far away
        }
    }

    private void ShowNextDialogue()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
        }
        else
        {
            dialogueText.text = ""; // Clear text when done
            currentLineIndex = 0; // Reset dialogue
        }
    }
}
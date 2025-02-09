using UnityEngine;
using UnityEngine.UI; // For UI components

public class CharacterClick : MonoBehaviour
{
    public GameObject dialogueUI; // The Panel holding the dialogue box
    public Text dialogueText; // The Text component for dialogue
    public Button nextButton; // The button to go to the next line of dialogue
    public string[] dialogueLines; // Array of dialogue lines
    private int currentLine = 0; // Keeps track of the current line

    public float interactionRange = 5f; // Distance at which the player can interact with the character
    private Transform player; // Reference to the player's transform

    void Start()
    {
        // Initially hide the dialogue UI
        dialogueUI.SetActive(false);
        nextButton.onClick.AddListener(NextDialogue); // Set up button to progress dialogue
        player = Camera.main.transform; // Assuming the player is controlled by the camera
    }

    void Update()
    {
        // Calculate the distance between the player and the AI character
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // If the player is within the interaction range, check for a click
        if (distanceToPlayer <= interactionRange)
        {
            if (Input.GetMouseButtonDown(0)) // Check for mouse click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform) // Check if clicked on this character
                    {
                        StartConversation(); // Start the conversation
                    }
                }
            }
        }

        // Optionally, you can show a UI indicator (e.g., a button) when the player is within range
    }

    void StartConversation()
    {
        dialogueUI.SetActive(true); // Show the dialogue box
        ShowDialogue(); // Show the first line of dialogue
    }

    void ShowDialogue()
    {
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine]; // Update the text
            currentLine++;
        }
        else
        {
            EndConversation(); // End conversation when all lines are shown
        }
    }

    void NextDialogue()
    {
        ShowDialogue(); // Show next dialogue line when button is clicked
    }

    void EndConversation()
    {
        dialogueUI.SetActive(false); // Hide the dialogue box
        currentLine = 0; // Reset dialogue
    }
}

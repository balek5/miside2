using UnityEngine;
using UnityEngine.UI;

public class DisplayChoices : MonoBehaviour
{
    public Button choiceButton1;
    public Button choiceButton2;
    public string[] dialogueSentences;
    public string choice1Text;
    public string choice2Text;

    private void Start()
    {
        // Set up buttons with event listeners
        choiceButton1.onClick.AddListener(() => OnChoiceSelected(1));
        choiceButton2.onClick.AddListener(() => OnChoiceSelected(2));

        // Initially hide the choice buttons
        HideChoiceButtons();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show the choice buttons when the player gets close
            ShowChoices(choice1Text, choice2Text);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the choice buttons when the player moves away
            HideChoiceButtons();
        }
    }

    public void ShowChoices(string choice1, string choice2)
    {
        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);

        choiceButton1.GetComponentInChildren<Text>().text = choice1;
        choiceButton2.GetComponentInChildren<Text>().text = choice2;
    }

    private void HideChoiceButtons()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        // Handle the choice made by the player
        if (choiceIndex == 1)
        {
            // Continue with one branch of dialogue
            dialogueSentences = new string[] { "You chose option 1!", "This is the next sentence." };
        }
        else
        {
            // Continue with another branch of dialogue
            dialogueSentences = new string[] { "You chose option 2!", "This is another sentence." };
        }

        // Restart the dialogue with the new set of sentences
        StartDialogue();
    }

    private void StartDialogue()
    {
        // Implementation for starting the dialogue
    }
}
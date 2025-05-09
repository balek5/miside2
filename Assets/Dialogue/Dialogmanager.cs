using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private Button[] choiceButtons;
    [Header("Timing Settings")]
    [SerializeField] private float responseDisplayTime = 1.5f; // How long to show the response before returning to choices

    private Action<int> onChoiceSelected;
    private string[] currentChoices;
    private string[] currentResponses; // Responses corresponding to each choice
    private bool isShowingDialogue;
    private bool choiceMade = false;
    [SerializeField] private float typingSpeed = 0.05f;
    // Reference to TrustDefianceManager
    private TrustDefianceManager trustDefianceManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false); // Ensure choices panel starts OFF

        foreach (Button btn in choiceButtons)
        {
            btn.gameObject.SetActive(false); // Disable all buttons initially
        }

        // Get the TrustDefianceManager component
        trustDefianceManager = FindObjectOfType<TrustDefianceManager>();
        if (trustDefianceManager == null)
        {
            Debug.LogError("TrustDefianceManager not found! Please add it to the scene.");
        }
    }

    public void DisplayDialogueLine(string line)
    {
        dialogueText.text = line; // Display the line in the UI
    }

    /// <summary>
    /// Shows a single dialogue line without choices.
    /// </summary>
    public void ShowDialogue(string dialogue)
    {
        dialoguePanel.SetActive(true);
        choicesPanel.SetActive(false);
        dialogueText.text = dialogue;
    }

    /// <summary>
    /// Starts a dialogue sequence with choices.
    /// </summary>
    public void StartDialogue(
        string[] dialogueLines,
        string[] choices,
        bool[] isChoiceInteractable, // Determines if each choice is interactable
        string[] responses,
        Action<int> onChoiceSelectedCallback)
    {
        if (dialogueLines == null || choices == null || isChoiceInteractable == null || responses == null)
        {
            Debug.LogError("One or more parameters passed to StartDialogue are null!");
            return;
        }

        if (choices.Length != isChoiceInteractable.Length || choices.Length != responses.Length)
        {
            Debug.LogError("Mismatch in lengths of choices, isChoiceInteractable, or responses arrays!");
            return;
        }

        // Reset for new dialogue
        choiceMade = false;
        this.onChoiceSelected = onChoiceSelectedCallback;
        currentChoices = choices;
        currentResponses = responses;

        StartCoroutine(RunDialogueSequence(dialogueLines, isChoiceInteractable));
    }

    private IEnumerator RunDialogueSequence(string[] dialogueLines, bool[] isChoiceInteractable)
    {
        isShowingDialogue = true;
        dialoguePanel.SetActive(true);
        choicesPanel.SetActive(false);

        // Display the initial dialogue lines (if any).
        foreach (var line in dialogueLines)
        {
            dialogueText.text = line;
            yield return new WaitForSeconds(2f);
        }

        dialoguePanel.SetActive(false);

        // Show the choices.
        ShowChoices(currentChoices, isChoiceInteractable, OnChoiceSelected);

        isShowingDialogue = false;
    }

    /// <summary>
    /// Displays the given choices and assigns each button a listener that passes its index.
    /// </summary>
    public void ShowChoices(string[] choices, bool[] isChoiceInteractable, Action<int> choiceCallback)
    {
        choicesPanel.SetActive(true);
        choiceMade = false;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true); // Activate the button
                choiceButtons[i].GetComponentInChildren<Text>().text = choices[i]; // Set the text

                // Clear existing listeners to avoid duplication
                choiceButtons[i].onClick.RemoveAllListeners();

                // Add a new listener
                int index = i; // Capture index for the closure
                choiceButtons[i].onClick.AddListener(() =>
                {
                    if (choiceButtons[index].interactable) // Only respond if the button is interactable
                    {
                        choiceButtons[index].interactable = false; // Disable only this button
                        choiceCallback(index); // Invoke the callback with the selected index
                    }
                });

                // Set interactivity based on isChoiceInteractable
                choiceButtons[i].interactable = isChoiceInteractable[i];
                Debug.Log($"Button {i}: {(isChoiceInteractable[i] ? "Enabled" : "Disabled")}");
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false); // Deactivate unused buttons
            }
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"Choice {choiceIndex} selected.");

        // Hide the choices panel and show the dialogue panel with the response.
        choicesPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = currentResponses[choiceIndex];

        // Optionally invoke any external callback.
        onChoiceSelected?.Invoke(choiceIndex);

        // Update Trust/Defiance based on choice (you can modify this logic as per your choices)
        if (choiceIndex == 0) // Example: if the first choice is "Trust"
        {
            trustDefianceManager.ChangeTrust(10);  // Increase trust
        }
        else if (choiceIndex == 1) // Example: if the second choice is "Defy"
        {
            trustDefianceManager.ChangeDefiance(10); // Increase defiance
        }

        // Wait for a moment before checking if there are remaining active choices.
        StartCoroutine(WaitAndReturnToChoices());
    }

    /// <summary>
    /// Waits for a short period, then checks if any choice buttons remain interactable.
    /// If so, the choices panel is re-shown; otherwise, the dialogue is closed.
    /// </summary>
    private IEnumerator WaitAndReturnToChoices()
    {
        yield return new WaitForSeconds(responseDisplayTime);

        dialoguePanel.SetActive(false);

        bool anyButtonStillActive = false;
        for (int i = 0; i < currentChoices.Length; i++)
        {
            if (i < choiceButtons.Length && choiceButtons[i].gameObject.activeSelf && choiceButtons[i].interactable)
            {
                anyButtonStillActive = true;
                break;
            }
        }

        if (anyButtonStillActive)
        {
            choicesPanel.SetActive(true);
        }
        else
        {
            CloseDialogue();
        }
    }
    public void ShowMessage(string message)
    {
        ShowDialogue(message); // Reuse the existing ShowDialogue method for single-line messages
    }
    /// <summary>
    /// Closes both the dialogue and choices panels.
    /// </summary>
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
    }
    private IEnumerator TypeText(string fullText)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = string.Empty;

        foreach (char c in fullText.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1f); // Pause after the text is fully typed
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger Instance;
    [Header("UI References")]
    [SerializeField] private float interactionRadius = 3f;
    [SerializeField] private Text taskText;
    [Header("Doll Settings")]
    [SerializeField] private int totalDolls = 2;
    private bool isShowingTaskMessage = false;
    private bool playerInRange;
    private int dollsFound = 0;
    private bool[] dollsCounted;
    private bool allDollsFound = false;
    private FirstPersonController fpsController;
    private bool isDialogueActive = false;

    // Initial dialogue
    private readonly string[] initialDialogue = {
        "Oh! You're finally awake! I was afraid you wouldn’t wake up. Don’t be scared… you’re safe with me."
    };
    private readonly string[] initialChoices = {
        "Who are you?", "What is This Place?", "Can I leave?"
    };
    private readonly string[] initialResponses = { 
        "Mio: Me? I'm Mio. That's all you need to know. I know you, too....",
        "Mio: This place is my special world—a sanctuary of memories. Don't try to leave.",
        "Mio: Leaving? Why would you want to leave? I can make it so you never have to go..."
    };

    // Final dialogue (after completing the quest)
    private readonly string[] truthDialogue = {
        "Mio sighs, tilting her head as if disappointed.",
        "\"Truth?\" she whispers. \"The truth is… you put yourself here.\"",
        "Do you really want to leave?"
    };
    private readonly string[] endingDialogue = {
        "Goodbye my dear Player... you were the best...",
        "One last chance to stay... Do you want to stay?"
    };
    private readonly string[] finalDecisionDialogue = {
        "Are you sure? You say that now, but… what if you forget again?",
        "I refuse to play your game! (Try to reject the cycle.)",
        "Mio frowns. \"Oh? And what makes you think you have a choice?\"",
        "You never had a chance of running from my world... Now you will be my Toy forever..."
    };
    private readonly string[] stayEndingDialogue = {
        "\"You don't belong out there anymore, do you?\"",
        "\"Don't worry my dear Player... You won't regret this decision...\"",
        "\"You'll find true happiness here...\""
    };
    private readonly string[] finalChoices = {
        "Yes", "No"
    };
    private readonly string[] finalResponses = {
        "Yes you are crazy and manipulative! I miss my friends and family! This is my final goodbye!",
        "I don't want to stay..."
    };

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

        fpsController = FindObjectOfType<FirstPersonController>();
        dollsCounted = new bool[totalDolls];
    }

    private void Start()
    {
        InitializeTaskText();
        StartCoroutine(DelayedInitialDialogue());
    }

    void InitializeTaskText()
    {
        if (taskText != null)
        {
            taskText.text = $"Dolls Found: {dollsFound}/{totalDolls}";
        }
        else
        {
            Debug.LogError("Task Text reference missing!");
        }
    }

    public void OnDollFound(int dollIndex)
    {
        if (dollIndex < 0 || dollIndex >= totalDolls)
        {
            Debug.LogError($"Invalid dollIndex: {dollIndex}");
            return;
        }

        if (!dollsCounted[dollIndex])
        {
            dollsCounted[dollIndex] = true;
            dollsFound++;
            UpdateTaskText();

            if (dollsFound >= totalDolls)
            {
                allDollsFound = true;
                EnableFinalConversation();
            }
        }
    }

    public bool AllDollsFound()
    {
        return dollsFound >= totalDolls;
    }

    private void UpdateTaskText()
    {
        if (isShowingTaskMessage) return;

        if (taskText != null)
        {
            if (allDollsFound)
            {
                taskText.text = "Quest Completed!";
            }
            else
            {
                taskText.text = $"Dolls Found: {dollsFound}/{totalDolls}";
            }
        }
    }

    private IEnumerator DelayedInitialDialogue()
    {
        yield return new WaitForSeconds(2f);
        StartDialogueSequence(initialDialogue, initialChoices, initialResponses);
    }

    private void StartDialogueSequence(string[] dialogueLines, string[] choices, string[] responses)
    {
        fpsController?.ToggleNPCInteraction(true);
        ShowDialogueUI();

        bool[] interactableChoices = new bool[choices.Length];
        for (int i = 0; i < interactableChoices.Length; i++)
        {
            interactableChoices[i] = true;
        }

        DialogueManager.Instance.StartDialogue(
            dialogueLines,
            choices,
            interactableChoices,
            responses,
            HandleChoice
        );
    }

    private void HandleChoice(int choiceIndex)
    {
        if (allDollsFound)
        {
            HandleFinalChoice(choiceIndex);
        }
        else
        {
            StartCoroutine(StartDollFindingTaskWithDelay());
        }

        fpsController?.ToggleNPCInteraction(false);
        HideDialogueUI();
    }

    private IEnumerator StartDollFindingTaskWithDelay()
    {
        isShowingTaskMessage = true;
        yield return new WaitForSeconds(1f);

        if (taskText != null)
        {
            taskText.text = "Look around the map to find dolls and interact with them.";
        }

        yield return new WaitForSeconds(5f);
        isShowingTaskMessage = false;
        UpdateTaskText();
    }

    public void EnableFinalConversation()
    {
        Debug.Log("All dolls found! Final conversation unlocked.");
        UpdateTaskText();
        StartDialogueSequence(truthDialogue, finalChoices, finalResponses);
    }

    private void HandleFinalChoice(int choiceIndex)
    {
        switch (choiceIndex)
        {
            case 0: // Yes - Want to leave
                StartCoroutine(ProcessLeavingChoice());
                break;
            case 1: // No - Don't want to stay
                StartCoroutine(ProcessStayChoice());
                break;
        }
    }

    private IEnumerator ProcessLeavingChoice()
    {
        yield return StartCoroutine(ShowDialogueSequence(endingDialogue, finalChoices));

        int finalChoice = -1;
        DialogueManager.Instance.StartDialogue(
            new string[] { "System: One last chance to stay..." },
            finalChoices,
            new bool[] { true, true },
            finalResponses,
            choice => finalChoice = choice
        );

        yield return new WaitUntil(() => finalChoice != -1);

        if (finalChoice == 0) // Yes - Stay
        {
            yield return StartCoroutine(ShowStayEnding());
        }
        else // No - Leave
        {
            QuitGame();
        }
    }

    private IEnumerator ProcessStayChoice()
    {
        foreach (string line in finalDecisionDialogue)
        {
            DialogueManager.Instance.ShowMessage(line);
            yield return new WaitForSeconds(3f);
        }

        int stayChoice = -1;
        DialogueManager.Instance.StartDialogue(
            new string[] { "Mio: Do you want to stay HERE?" },
            finalChoices,
            new bool[] { true, true },
            new string[] { "No, I want to leave", "Yes, I'll stay" },
            choice => stayChoice = choice
        );

        yield return new WaitUntil(() => stayChoice != -1);

        if (stayChoice == 0) // Leave
        {
            QuitGame();
        }
        else // Stay
        {
            yield return StartCoroutine(ShowStayEnding());
        }
    }

    private IEnumerator ShowStayEnding()
    {
        foreach (string line in stayEndingDialogue)
        {
            DialogueManager.Instance.ShowMessage(line);
            yield return new WaitForSeconds(3f);
        }

        taskText.text = "ENDING: Eternal Companion";
        fpsController?.ToggleNPCInteraction(true);
    }

    private IEnumerator ShowDialogueSequence(string[] dialogue, string[] choices)
    {
        foreach (string line in dialogue)
        {
            DialogueManager.Instance.ShowMessage(line);
            yield return new WaitForSeconds(3f);
        }

        int choice = -1;
        DialogueManager.Instance.StartDialogue(
            new string[0],
            choices,
            new bool[] { true, true },
            choices,
            c => choice = c
        );

        yield return new WaitUntil(() => choice != -1);
    }

    private void QuitGame()
    {
        taskText.text = "ENDING: Freedom";
        StartCoroutine(DelayedQuit());
    }

    private IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(3f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.Instance.CloseDialogue();
            fpsController?.ToggleNPCInteraction(false);
            HideDialogueUI();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !isDialogueActive)
        {
            isDialogueActive = true;

            if (allDollsFound)
            {
                EnableFinalConversation();
            }
            else
            {
                StartDialogueSequence(initialDialogue, initialChoices, initialResponses);
            }
        }
    }

    public void CloseDialogue()
    {
        Debug.Log("Closing dialogue triggered by DialogueTrigger.");
        DialogueManager.Instance.CloseDialogue();
        isDialogueActive = false;
    }

    public void ShowDialogueUI()
    {
        isDialogueActive = true;
    }

    public void HideDialogueUI()
    {
        isDialogueActive = false;
    }
}
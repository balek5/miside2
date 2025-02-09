using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
public class DollAi_alpha : MonoBehaviour
{
    public int dollIndex = 0;
    public float interactionDistance = 3f;
    private BoxCollider interactionCollider;
    private bool[] pathCompleted = new bool[3];
    private bool allPathsCompleted = false;
    private bool dollCounted = false;
    private bool isInteracting = false;
    private bool hasFirstInteraction = false;
    public Text dialogueText;
    private readonly string[][] paths = {
        // Path 0
        new string[] {
            "Are you sure? Even when you know what she's done to the others?",
            "What others?",
            "The ones who came before you...",
            "I'm not like them!",
            " \"We’ll see. But tell me this—what are you willing to sacrifice to escape?\n",
            "Everything!"
        },
        // Path 1
        new string[] {
            "Do you remember Something Player?",
            "I remember… something",
            "Good. Then you know why you’re here. But do you have the strength to break it this time"
        },
        // Path 2
        new string[] {
            "Do you remember me, Mio?",
            "Who is Mio?",
            "Mio isn't real...",
            "Good. Then you know why you’re here. But do you have the strength to break it this time?"
        }
    };

    void Start()
    {
        interactionCollider = GetComponent<BoxCollider>();
        if (interactionCollider != null)
        {
            interactionCollider.isTrigger = true;
            interactionCollider.size = Vector3.one * interactionDistance * 2;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracting && !allPathsCompleted)
        {
            Debug.Log($"Player entered Alpha Doll (Index: {dollIndex}) trigger");
            StartCoroutine(DialogueFlow());
        }
    }
     IEnumerator DialogueFlow()
    {
        isInteracting = true;

        // ==== COUNT THE DOLL IMMEDIATELY ====
        if (!dollCounted)
        {
            CountDoll();
        }

        while (!allPathsCompleted)
        {
            int pathChoice = -1;

            string[] initialLines = hasFirstInteraction ? 
                new string[0] : 
                new string[] { "Alpha Doll: 'Your fate is sealed within these eyes...'" };

            DialogueManager.Instance.StartDialogue(
                initialLines,
                new string[] { paths[0][0], paths[1][0], paths[2][0] }, // Dialogue options content
                new bool[] { !pathCompleted[0], !pathCompleted[1], !pathCompleted[2] },
                new string[] { paths[0][0], paths[1][0], paths[2][0] }, // Now using actual dialogue lines as labels
                choice => pathChoice = choice
            );
            hasFirstInteraction = true;
            yield return new WaitUntil(() => pathChoice != -1);

            if (pathChoice >= 0 && pathChoice < 3 && !pathCompleted[pathChoice])
            {
                yield return StartCoroutine(ProcessPath(pathChoice));
                pathCompleted[pathChoice] = true;
            }

            allPathsCompleted = pathCompleted[0] && pathCompleted[1] && pathCompleted[2];
        }

        isInteracting = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!isInteracting && FirstPersonController.Instance && InRange() && 
            other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !allPathsCompleted)
        {
            Debug.Log($"Interacting with Alpha Doll (Index: {dollIndex})");
            StartCoroutine(DialogueFlow());
        }
    }
    
    IEnumerator ClearDialogueText()
    {
        yield return new WaitForSeconds(2f); // Wait 2 seconds before clearing text
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    IEnumerator ProcessPath(int pathIndex)
    {
        string[] path = paths[pathIndex];
        for (int i = 1; i < path.Length; i++)
        {
            // Set the UI text to the current dialogue line
            if (dialogueText != null)
            {
                dialogueText.text = path[i];
            }

            // Wait for player to press Space or Enter to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        }

        // Clear text after dialogue ends
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    public void StartDialogue(string[] initialLines, string[] options, bool[] availableOptions, string[] optionLabels, System.Action<int> onChoiceSelected)
    {
        // Your existing logic for starting dialogue
    }
    bool InRange()
    {
        return Vector3.Distance(transform.position, 
               FirstPersonController.Instance.transform.position) < interactionDistance;
    }

    void CountDoll()
    {
        if (!dollCounted)
        {
            Debug.Log($"Alpha Doll {dollIndex} has been counted.");
            DialogueTrigger.Instance.OnDollFound(dollIndex);
            dollCounted = true;
        }
    }
    
}
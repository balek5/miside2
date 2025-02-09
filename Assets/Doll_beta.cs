using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class DollAI_Beta : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public int dollIndex = 1; // Ensure this is unique
    private bool hasInteracted = false;
    private bool dollCounted = false;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform == player && !hasInteracted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Interacting with Beta Doll (Index: {dollIndex})");
            hasInteracted = true;
            StartDialogueInteraction();
        }
    }

    void StartDialogueInteraction()
    {
        DialogueManager.Instance.StartDialogue(
            new string[] { "That's Rosy. She doesn't like strangers..." },
            new string[] { "Trust Mio", "Why should I trust Mio? What’s her deal?", "Hug the Plushie" },
            new bool[] { true, true, true }, // All choices are interactable
            new string[] { "Mio? Oh, Player… you shouldn’t trust her so easily. But if you insist, go ahead", "Mio isn’t what she seems. She’s… different. Dangerous. But if you’re clever, you might survive her.", "Voice: Now she’s attached to you. Good luck getting rid of her." },
            choiceIndex => StartCoroutine(FinishInteraction(choiceIndex))
        );
    }

    IEnumerator FinishInteraction(int choiceIndex)
    {
        // Process the chosen dialogue path
        yield return StartCoroutine(ProcessDialogueChoice(choiceIndex));

        // Count the doll if not already counted
        if (!dollCounted)
        {
            CountDoll();
        }

        // Check if all dolls have been found
        if (DialogueTrigger.Instance.AllDollsFound())
        {
            Debug.Log("All dolls found! Triggering final dialogue.");
            yield return new WaitForSeconds(1f); // Small delay before final dialogue
            DialogueTrigger.Instance.EnableFinalConversation();
        }
    }

    IEnumerator ProcessDialogueChoice(int choiceIndex)
    {
        // Simulate processing the chosen dialogue path
        Debug.Log($"Processing choice: {choiceIndex}");
        yield return new WaitForSeconds(2f); // Simulate dialogue duration
    }

    void CountDoll()
    {
        if (!dollCounted)
        {
            Debug.Log($"Beta Doll {dollIndex} has been counted.");
            DialogueTrigger.Instance.OnDollFound(dollIndex);
            dollCounted = true;
        }
    }
}
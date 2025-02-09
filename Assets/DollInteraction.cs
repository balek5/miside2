using UnityEngine;

public class DollInteraction : MonoBehaviour
{
    public Transform player;             // Reference to the player object
    public Transform head;               // Reference to the doll's head for rotation
    public float interactionDistance = 3f; // Distance required for interaction
    public int dollIndex;               // Unique index for this doll (Alpha: 1, Beta: 2, Gamma: 3)

    private bool hasInteracted = false;  // Prevents repeated interactions for this doll
    private bool dollCounted = false;    // Ensures this doll is counted only once

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
            {
                Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
                return;
            }
        }

        Debug.Log($"Doll {dollIndex} initialized.");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Make the doll look at the player if they are close enough
        if (distance < 5f)
        {
            LookAtPlayer();
        }

        // Handle player interaction
        if (distance < interactionDistance && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            Debug.Log($"Player interacted with Doll {dollIndex}.");
            StartInteraction();
        }
    }

    void LookAtPlayer()
    {
        // Rotate the doll's head to face the player smoothly
        Vector3 lookDir = player.position - head.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        head.rotation = Quaternion.Lerp(head.rotation, targetRotation, Time.deltaTime * 3f);
    }

    void StartInteraction()
    {
        if (!hasInteracted)
        {
            hasInteracted = true; // Prevent multiple interactions

            if (!dollCounted)
            {
                dollCounted = true;
                Debug.Log($"Doll {dollIndex} counted. Sending to DialogueTrigger.");

                // Call OnDollFound once
                if (DialogueTrigger.Instance != null)
                {
                    DialogueTrigger.Instance.OnDollFound(dollIndex);
                }
                else
                {
                    Debug.LogError("DialogueTrigger.Instance is null!");
                }
            }
            else
            {
                Debug.LogWarning($"Doll {dollIndex} was already counted, skipping interaction.");
            }

            DialogueManager.Instance.StartDialogue(
                new string[] { $"Doll {dollIndex}: I remember everything... even the secrets you hide." },
                new string[] { "Listen Closely", "Question Her", "Walk Away" },
                new bool[] { true, true, true },  // All choices are interactable
                new string[] {
                    $"Doll {dollIndex}: Your trust opens hidden doors.",
                    $"Doll {dollIndex}: Curiosity can be dangerous...",
                    $"Doll {dollIndex}: Sometimes ignorance is bliss."
                },
                OnChoiceSelected
            );


        }
        else
        {
            Debug.LogWarning($"Doll {dollIndex} interaction already happened, ignoring.");
        }
    }


    void OnChoiceSelected(int choiceIndex)
    {
        switch (choiceIndex)
        {
            case 0: // Listen Closely
                Debug.Log($"Doll {dollIndex}: Listen Closely selected!");
                break;
            case 1: // Question Her
                Debug.Log($"Doll {dollIndex}: Question Her selected!");
                break;
            case 2: // Walk Away
                Debug.Log($"Doll {dollIndex}: Walk Away selected!");
                break;
        }

        // Mark the interaction as complete
        dollCounted = true; // Ensure the doll is counted after interaction
        Debug.Log($"Interaction complete for Doll {dollIndex}.");
    }
}

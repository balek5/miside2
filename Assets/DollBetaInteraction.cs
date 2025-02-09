using UnityEngine;
using System.Collections;

public class DollBetaInteraction : MonoBehaviour
{
    public Transform player;
    public Transform head;
    public float interactionDistance = 3f;
    public int dollIndex;

    private bool hasInteracted = false;
    private bool dollCounted = false;
    private Coroutine finishInteractionCoroutine = null;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("Player not found! Ensure your player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (dollCounted) return;

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < 5f)
        {
            LookAtPlayer();
        }

        if (distance < interactionDistance && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            StartInteraction();
        }
    }

    void LookAtPlayer()
    {
        Vector3 lookDir = player.position - head.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        head.rotation = Quaternion.Lerp(head.rotation, targetRotation, Time.deltaTime * 3f);
    }

    void StartInteraction()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;

            if (!dollCounted)
            {
                dollCounted = true;
                DialogueTrigger.Instance.OnDollFound(dollIndex);
            }

            DialogueManager.Instance.StartDialogue(
                new string[] { "Beta Doll: I remember everything... even the secrets you hide." },
                new string[] { "Listen Closely", "Question Her", "Walk Away" },
                new bool[] { true, true, true },
                new string[] {
                    "Beta Doll: Your trust opens hidden doors.",
                    "Beta Doll: Curiosity can be dangerous...",
                    "Beta Doll: Sometimes ignorance is bliss."
                },
                OnChoiceSelected
            );
        }
    }

    void OnChoiceSelected(int choiceIndex)
    {
        switch (choiceIndex)
        {
            case 0: 
                Debug.Log("Beta Doll: Listen Closely selected!");
                InfluenceMeter.Instance?.AdjustInfluence(1);
                break;
            case 1: 
                Debug.Log("Beta Doll: Question Her selected!");
                InfluenceMeter.Instance?.AdjustInfluence(0);
                break;
            case 2: 
                Debug.Log("Beta Doll: Walk Away selected!");
                InfluenceMeter.Instance?.AdjustInfluence(-1);
                break;
        }

        if (!dollCounted)
        {
            dollCounted = true;

            if (finishInteractionCoroutine == null)
            {
                finishInteractionCoroutine = StartCoroutine(FinishInteraction());
            }
        }
    }

    IEnumerator FinishInteraction()
    {
        yield return new WaitForSeconds(3f);

        if (!dollCounted)
        {
            dollCounted = true;
            DialogueTrigger.Instance.OnDollFound(dollIndex);
        }

        finishInteractionCoroutine = null;
    }
}

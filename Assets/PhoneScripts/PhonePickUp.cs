using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PhonePickup : MonoBehaviour
{
    public Transform phoneHolder; // Reference to the PhoneHolder
    public GameObject phoneModel; // Reference to the phone model
    public PhoneUI phoneUI; // Reference to the PhoneUI script
    public CanvasGroup phoneCanvasGroup; // Reference to the CanvasGroup of the UI

    private bool isHoldingPhone = false;
    private float fadeDuration = 0.5f; // Duration for UI fade-in/out
    public Animator phoneAnimator; // Reference to the Animator

    private bool isAnimatingPickup = false; // Flag to check if the pickup animation is playing
    public PlayableDirector timeline;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHoldingPhone)
            {
                PickUpPhone();
            }
            else
            {
                PutDownPhone();
            }
        }

        // When holding the phone, allow interaction with the phone
        if (isHoldingPhone && Input.GetKeyDown(KeyCode.F))
        {
            InteractWithPhone(); // Call your interaction method here
        }
    }

    // Example interaction method when pressing "F"
    void InteractWithPhone()
    {
        phoneUI.StartDownload(); // Assuming this starts a phone download process or other UI interaction
        Debug.Log("Interacting with the phone!");
    }

    // Coroutine to fade the Canvas Group (UI)
    IEnumerator FadeCanvas(CanvasGroup canvas, float targetAlpha, float duration)
    {
        float startAlpha = canvas.alpha;
        float time = 0;

        // Debugging output for canvas fade
        Debug.Log($"Starting fade: {startAlpha} -> {targetAlpha}");

        // Fade in/out logic
        while (time < duration)
        {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvas.alpha = targetAlpha;

        // Debugging after fade is complete
        Debug.Log($"Fade complete: {canvas.alpha}");
    }

    // Pick up the phone (start move animation and UI fade)
    void PickUpPhone()
    {
        if (isAnimatingPickup) return; // Prevent re-triggering while the animation is playing
        isAnimatingPickup = true;

        // Trigger the phone's "PickUp" animation
        phoneAnimator.SetTrigger("PickUp");

        // Move phone to holder position with a slight offset if needed
        phoneModel.transform.SetParent(phoneHolder);
        phoneModel.transform.localPosition = Vector3.zero; // Make sure this is appropriate for the phone's position in hand
        phoneModel.transform.localRotation = Quaternion.identity; // Reset rotation, adjust if the hand needs a different rotation
        isHoldingPhone = true;

        // Fade the UI Canvas in concurrently with the phone animation
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(phoneCanvasGroup, 1, fadeDuration));

        // After the pickup animation finishes, allow the UI to fade
        StartCoroutine(ResetPickupFlagAfterAnimation("PickUp"));
    }

    // Put down the phone (start move animation and UI fade)
    void PutDownPhone()
    {
        // Trigger the phone's "MovePhone" animation
        phoneAnimator.SetTrigger("MovePhone");

        // Move phone back to its original position (if necessary)
        phoneModel.transform.SetParent(null);
        isHoldingPhone = false;

        // Fade the UI Canvas out concurrently with the phone animation
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(phoneCanvasGroup, 0, fadeDuration));
    }

    // Coroutine to stop the "MovePhone" animation after 1 second
    IEnumerator StopMovePhoneAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the "MovePhone" trigger after a delay to return to idle state
        phoneAnimator.ResetTrigger("MovePhone");

        // Optionally, set the animator to idle or another state if needed
        phoneAnimator.SetTrigger("Idle"); // Trigger idle animation after move
    }

    // Coroutine to reset the pickup flag after the animation is done
    IEnumerator ResetPickupFlagAfterAnimation(string animationTrigger)
    {
        // Wait for the animation to finish, assuming it lasts 1 second
        yield return new WaitForSeconds(1f); // Adjust if your animation duration differs
        isAnimatingPickup = false;
        Debug.Log($"{animationTrigger} animation finished.");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            timeline.Play();    
        }
    }
}


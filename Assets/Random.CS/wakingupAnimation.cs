using UnityEngine;
using System.Collections;

public class WakeUpAnimation : MonoBehaviour
{
    public float wakeUpTime = 3.0f;  // Animation duration
    public Transform playerHead;  // Drag your player's head or camera holder here in the Inspector
    private Quaternion startRotation;
    private Quaternion endRotation;
    private Vector3 originalLocalPosition; // Stores original local position
    private Quaternion originalLocalRotation; // Stores original local rotation

    void Start()
    {
        // Store the original local position & rotation before detaching
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;

        // Detach the camera so it moves freely during animation
        transform.SetParent(null);

        // Define animation start and end rotation
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(10, 0, 0); // Slightly looking forward

        // Start wake-up animation
        StartCoroutine(WakeUpSequence());
    }

    IEnumerator WakeUpSequence()
    {
        float elapsedTime = 0;

        while (elapsedTime < wakeUpTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / wakeUpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // Ensure final rotation is correct

        // **Reattach camera to player's head smoothly**
        transform.SetParent(playerHead);
        transform.localPosition = originalLocalPosition; // Reset position
        transform.localRotation = originalLocalRotation; // Reset rotation
    }
}
using UnityEngine;

public class RoomSwapper : MonoBehaviour
{
    public GameObject room1;
    public GameObject room3;
    public float swapDelay = 1f;
    private bool isSwapping = false;

    // Sound Effects
    public AudioClip enterSound;
    public AudioClip exitSound;
    public AudioClip swapSound;
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // Play enter sound
        if (enterSound != null)
        {
            audioSource.PlayOneShot(enterSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || isSwapping) return;
        
        // Play exit sound
        if (exitSound != null)
        {
            audioSource.PlayOneShot(exitSound);
        }
        
        StartCoroutine(SwapAfterDelay());
    }

    private System.Collections.IEnumerator SwapAfterDelay()
    {
        isSwapping = true;

        // Capture positions
        Vector3 r1Pos = room1.transform.position;
        Quaternion r1Rot = room1.transform.rotation;
        Vector3 r3Pos = room3.transform.position;
        Quaternion r3Rot = room3.transform.rotation;

        yield return new WaitForSeconds(swapDelay);

        // Play swap sound
        if (swapSound != null)
        {
            audioSource.PlayOneShot(swapSound);
        }

        // Perform swap
        Transform parent = room1.transform.parent;
        room1.transform.SetParent(null, true);
        room3.transform.SetParent(null, true);

        room1.transform.SetPositionAndRotation(r3Pos, r3Rot);
        room3.transform.SetPositionAndRotation(r1Pos, r1Rot);

        room1.transform.SetParent(parent, true);
        room3.transform.SetParent(parent, true);

        isSwapping = false;
    }
}
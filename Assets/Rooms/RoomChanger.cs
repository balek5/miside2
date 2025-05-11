using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;
public class RoomSwapper : MonoBehaviour
{
    public GameObject room1;
    public GameObject room3;
    public float swapDelay = 1f;
    private bool isSwapping = false;

    public AudioClip enterSound;
    public AudioClip exitSound;
    public AudioClip swapSound;
    private AudioSource audioSource;

    public NavMeshSurface navMeshSurface; // Add this in the inspector

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (enterSound != null)
            audioSource.PlayOneShot(enterSound);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || isSwapping) return;

        if (exitSound != null)
            audioSource.PlayOneShot(exitSound);

        StartCoroutine(SwapAfterDelay());
    }

    private IEnumerator SwapAfterDelay()
    {
        isSwapping = true;

        yield return new WaitForSeconds(swapDelay);

        // Play swap sound
        if (swapSound != null)
            audioSource.PlayOneShot(swapSound);

        // Save world positions and rotations
        Vector3 room1WorldPos = room1.transform.position;
        Quaternion room1WorldRot = room1.transform.rotation;

        Vector3 room3WorldPos = room3.transform.position;
        Quaternion room3WorldRot = room3.transform.rotation;

        // Detach from parents (so SetPositionAndRotation uses world space correctly)
        Transform room1Parent = room1.transform.parent;
        Transform room3Parent = room3.transform.parent;

        room1.transform.SetParent(null, true);
        room3.transform.SetParent(null, true);

        // 🔄 SWAP — Position AND Rotation
        room1.transform.SetPositionAndRotation(room3WorldPos, room3WorldRot);
        room3.transform.SetPositionAndRotation(room1WorldPos, room1WorldRot);

        // Reattach
        room1.transform.SetParent(room1Parent, true);
        room3.transform.SetParent(room3Parent, true);

    
         room1.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
         room3.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        // ✅ Rebuild NavMesh if using AI
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }

        Debug.Log("After Swap — Room1 rot: " + room1.transform.rotation.eulerAngles + ", Room3 rot: " + room3.transform.rotation.eulerAngles);

        isSwapping = false;
    }


}

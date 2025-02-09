using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform playerCamera; // Assign the player's camera in the Inspector

    void LateUpdate()
    {
        // Make the Canvas face the camera
        transform.LookAt(transform.position + playerCamera.forward);
    }
}
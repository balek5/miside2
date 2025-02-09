using UnityEngine;

public class BillboardText : MonoBehaviour
{
    public Camera playerCamera;  // Reference to the player's camera

    void Update()
    {
        // Make the text always face the camera
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Keep it upright
        }
    }
}
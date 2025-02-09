using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player; // The player or object the camera follows
    public float distance = 5f; // Normal distance from the player
    public float smoothSpeed = 0.125f; // Smoothing speed
    public LayerMask collisionLayers; // Define the layers of objects to collide with

    private Vector3 currentVelocity;

    void Update()
    {
        Vector3 desiredPosition = player.position - player.forward * distance;
        RaycastHit hit;

        // Cast a ray from the player's position to the desired camera position
        if (Physics.Raycast(player.position, (desiredPosition - player.position).normalized, out hit, distance, collisionLayers))
        {
            // If the ray hits something, move the camera closer to the player
            desiredPosition = hit.point;
        }

        // Smoothly move the camera to the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
    }
}
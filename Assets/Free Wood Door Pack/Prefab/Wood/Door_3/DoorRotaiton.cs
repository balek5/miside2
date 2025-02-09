using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    public float openAngle = 90f;  // Angle to rotate the door to open (in degrees)
    public float rotationSpeed = 2f;  // Speed of the rotation
    private bool isOpen = false;  // Whether the door is open or closed
    private Quaternion closedRotation;  // The closed rotation (initial rotation)
    private Quaternion openRotation;  // The open rotation (rotated by openAngle)

    private void Start()
    {
        // Store the initial rotation as the closed position
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;
    }

    private void Update()
    {
        // Log current rotation to debug if it's changing
        Debug.Log("Current Rotation: " + transform.rotation.eulerAngles);

        // Rotate the door smoothly to the target position (open or closed)
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Call this method to toggle the door state (open/close)
    public void ToggleDoor()
    {
        isOpen = !isOpen;
        Debug.Log("Door toggled. Is Open: " + isOpen);  // Log the door state when toggled
    }
}
using UnityEngine;

public class DoorTriggerController : MonoBehaviour
{
    public GameObject door; // Reference to the door prefab or the door object in the scene
    private Animator doorAnimator; // Reference to the door's Animator
    private bool isOpen = false; // Track the state of the door

    private void Start()
    {
        // Find the Animator component attached to the door object
        if (door != null)
        {
            doorAnimator = door.GetComponent<Animator>(); // Access Animator from the door prefab
        }
        else
        {
            Debug.LogError("Door object is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && doorAnimator != null)
        {
            // Open the door when the player enters the trigger area
            if (!isOpen)
            {
                doorAnimator.SetBool("isOpen", true); // Set the door to open
                isOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && doorAnimator != null)
        {
            // Close the door when the player exits the trigger area
            if (isOpen)
            {
                doorAnimator.SetBool("isOpen", false); // Set the door to close
                isOpen = false;
            }
        }
    }
}

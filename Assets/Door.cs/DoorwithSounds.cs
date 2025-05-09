using UnityEngine;

public class DoorWithSound : MonoBehaviour
{
    public AudioSource doorAudioSource;  // Reference to the AudioSource component
    public AudioClip openSound;          // The sound when the door opens
    public AudioClip closeSound;         // The sound when the door closes
    private bool isOpen = false;         // Track if the door is open or closed

    void Start()
    {
        // Ensure that the AudioSource component is attached to the door
        if (doorAudioSource == null)
        {
            doorAudioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))  // Change to your desired key
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        // Toggle the door state
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        // Play the door opening sound
        doorAudioSource.clip = openSound;
        doorAudioSource.Play();

        // Your door opening code (e.g., animation, movement)
        // For example: doorAnimator.SetTrigger("Open");

        isOpen = true;
    }

    void CloseDoor()
    {
        // Play the door closing sound
        doorAudioSource.clip = closeSound;
        doorAudioSource.Play();

        // Your door closing code (e.g., animation, movement)
        // For example: doorAnimator.SetTrigger("Close");

        isOpen = false;
    }
}


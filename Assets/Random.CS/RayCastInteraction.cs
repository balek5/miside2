using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public Camera playerCamera;  // Reference to the player's camera
    public float interactionDistance = 5f;  // Maximum distance for raycast interaction
    public LayerMask interactableLayer;  // Layer mask to specify which objects can be interacted with

    private void Update()
    {
        // Cast a ray from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Log the raycast direction for debugging
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // If the ray hits an object, check if it's a door
            Debug.Log("Raycast hit something: " + hit.collider.name);  // Log what the ray hits

            DoorRotation door = hit.collider.GetComponent<DoorRotation>();

            if (door != null)
            {
                // Log if the door is found
                Debug.Log("Looking at door: " + hit.collider.name);

                // Check if the player presses the "G" key
                if (Input.GetKeyDown(KeyCode.G))  // Change to "G" key
                {
                    // Toggle the door (open/close)
                    Debug.Log("Toggling door state.");
                    door.ToggleDoor();
                }
            }
            else
            {
                // Log if the ray hits something that is not a door
                Debug.Log("Hit object is not a door.");
            }
        }
        else
        {
            // Log if the raycast doesn't hit anything
            Debug.Log("Raycast did not hit anything.");
        }
    }
}
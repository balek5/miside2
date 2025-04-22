using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoomTrigger : MonoBehaviour
{
    [Tooltip("Index into RoomCubeController.roomLayouts")]
    public int roomIndex = 0;

    private void Awake()
    {
        // Ensure this is a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Find the controller (you can cache this reference if you like)
        var controller = FindObjectOfType<RoomCubeController>();
        if (controller != null)
            controller.SetRoom(roomIndex);
    }
}
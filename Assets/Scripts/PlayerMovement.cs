using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;

    void Update()
    {
        // Get movement input
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Calculate movement direction
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Apply movement
        controller.Move(move * speed * Time.deltaTime);
    }
}

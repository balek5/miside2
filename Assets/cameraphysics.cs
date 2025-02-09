using UnityEngine;

public class CameraPhysics : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevent the Rigidbody from rotating automatically
    }

    void Update()
    {
        // Simple movement logic
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }

        // Simple rotation logic
        float rotation = Input.GetAxis("Mouse X") * rotationSpeed;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
    }
}
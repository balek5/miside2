using UnityEngine;

public class Note : MonoBehaviour
{
    public float fallSpeed = 3f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Destroy if off-screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
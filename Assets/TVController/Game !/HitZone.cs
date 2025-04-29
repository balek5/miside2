using UnityEngine;

public class HitZone : MonoBehaviour
{
    public string keyToPress; // e.g., "q", "w", etc.

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Note") && Input.GetKeyDown(keyToPress))
        {
            GameManager.Instance.IncreaseScore();
            Destroy(other.gameObject);
        }
    }
}
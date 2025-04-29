using UnityEngine;

public class UINote : MonoBehaviour
{
    public float speed = 300f;       // How fast the note moves down
    public string key;               // The key this note is tied to ("q", "w", etc.)

    private RectTransform rect;

    // Y-position range of the hit zone (adjust based on your layout)
    private float hitZoneMinY = -300f;
    private float hitZoneMaxY = -250f;

    private bool hasBeenHit = false; // Prevent double scoring

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the note downward
        rect.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

        // If the correct key is pressed
        if (Input.GetKeyDown(key.ToLower()) && !hasBeenHit)
        {
            float yPos = rect.anchoredPosition.y;

            // Check if the note is within the hit zone
            if (yPos >= hitZoneMinY && yPos <= hitZoneMaxY)
            {
                hasBeenHit = true; // prevent repeated scoring
                Debug.Log("Good Hit on key: " + key.ToUpper());

                GameManager.Instance.IncreaseScore();
                GameManager.Instance.PlayHitSound();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Missed timing on key: " + key.ToUpper());
                GameManager.Instance.PlayMissSound();
            }
        }

        // Destroy and count as miss if note falls below screen
        if (rect.anchoredPosition.y < -400f && !hasBeenHit)
        {
            GameManager.Instance.PlayMissSound();
            Destroy(gameObject);
        }
    }
}
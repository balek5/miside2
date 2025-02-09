using UnityEngine;

public class InfluenceMeter : MonoBehaviour
{
    public static InfluenceMeter Instance;
    private int influence = 0;

    private void Awake()
    {
        // Set up the singleton instance.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists.
        }
    }

    public void AdjustInfluence(int delta)
    {
        influence += delta;
        Debug.Log("Influence adjusted. New value: " + influence);
    }
}
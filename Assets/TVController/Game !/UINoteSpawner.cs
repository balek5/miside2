using UnityEngine;

public class UINoteSpawner : MonoBehaviour
{
    public RectTransform[] laneParents; // Lane_Q, Lane_W, etc.
    public GameObject notePrefab;       // Your UI Note prefab
    public float spawnInterval = 1f;    // Seconds between spawns

    private string[] keys = { "q", "w", "e", "r" };

    void Start()
    {
        InvokeRepeating(nameof(SpawnNote), 1f, spawnInterval);
    }

    void SpawnNote()
    {
        int laneIndex = Random.Range(0, laneParents.Length);
        RectTransform lane = laneParents[laneIndex];

        GameObject note = Instantiate(notePrefab, lane);
        RectTransform noteRT = note.GetComponent<RectTransform>();

        // Set initial anchored position inside the lane
        noteRT.anchoredPosition = new Vector2(0f, 300f);

        // Assign key to note script
        UINote noteScript = note.GetComponent<UINote>();
        noteScript.key = keys[laneIndex];
    }
}
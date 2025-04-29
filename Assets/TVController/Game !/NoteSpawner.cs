using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;

    // Define horizontal X positions for Q, W, E, R lanes
    private float[] laneXPositions = new float[] { -3f, -1f, 1f, 3f };

    public float spawnY = 5f; // height where notes appear
    public float spawnInterval = 1f;

    void Start()
    {
        InvokeRepeating("SpawnNote", 1f, spawnInterval);
    }

    void SpawnNote()
    {
        int lane = Random.Range(0, laneXPositions.Length);
        Vector3 spawnPos = new Vector3(laneXPositions[lane], spawnY, 0f);
        Instantiate(notePrefab, spawnPos, Quaternion.identity);
    }
}
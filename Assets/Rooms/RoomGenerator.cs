using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Assign 3 room prefabs in inspector
    public Vector3[] spawnPositions; // Define 3 positions in inspector

    private GameObject[] spawnedRooms;

    void Start()
    {
        GenerateRooms();
    }

    void GenerateRooms()
    {
        spawnedRooms = new GameObject[roomPrefabs.Length];

        for (int i = 0; i < roomPrefabs.Length; i++)
        {
            GameObject room = Instantiate(roomPrefabs[i], spawnPositions[i], Quaternion.identity);
            spawnedRooms[i] = room;
        }
    }

    // Optional access
    public GameObject GetRoom(int index)
    {
        return spawnedRooms[index];
    }
}
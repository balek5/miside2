using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCubeController : MonoBehaviour
{
    [Header("All cubes in the flat")]
    public List<Transform> cubes;

    [Header("Layouts: list of positions per room (cubes.Count entries each)")]
    public List<Transform[]> roomLayouts;

    [Header("Animation settings")]
    public float moveDuration = 1f;

    private int currentRoom = -1;

    // Call this to switch to layout #roomIndex
    public void SetRoom(int roomIndex)
    {
        if (roomIndex == currentRoom || roomIndex < 0 || roomIndex >= roomLayouts.Count)
            return;

        Transform[] targets = roomLayouts[roomIndex];
        if (targets.Length != cubes.Count)
        {
            Debug.LogError("Room layout size mismatch!");
            return;
        }

        // Start moving each cube
        for (int i = 0; i < cubes.Count; i++)
            StartCoroutine(MoveCube(cubes[i], targets[i].position));

        currentRoom = roomIndex;
    }

    private IEnumerator MoveCube(Transform cube, Vector3 targetPos)
    {
        Vector3 start = cube.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            cube.position = Vector3.Lerp(start, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cube.position = targetPos;
    }
}
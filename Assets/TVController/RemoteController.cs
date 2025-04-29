// RemoteController.cs
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    public GameObject tvCanvas;

    private void OnMouseDown()
    {
        tvCanvas.SetActive(true);
        // Optional: Lock player movement here if needed
    }
}
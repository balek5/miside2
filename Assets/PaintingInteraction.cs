using UnityEngine;
using UnityEngine.UI; // If using UI for dialogue

public class PaintingInteraction : MonoBehaviour
{
    public string paintingMessage; // Customize the message for each painting
    private bool isLooking = false;

    void Update()
    {
        if (isLooking && Input.GetKeyDown(KeyCode.E)) // Press E to interact
        {
            ShowMessage();
        }
    }

    void OnMouseEnter()
    {
        isLooking = true;
        // Show UI prompt like "Press E to inspect"
    }

    void OnMouseExit()
    {
        isLooking = false;
        // Hide UI prompt
    }

    void ShowMessage()
    {
        Debug.Log(paintingMessage);
        // Trigger UI dialogue or creepy effect
    }
}
using UnityEngine;

public class CreepyPainting : MonoBehaviour
{
    public Sprite changedPainting;  // Drag the creepy version in Inspector
    private Sprite originalPainting;
    private SpriteRenderer spriteRenderer;
    private bool hasChanged = false; // Prevents rapid toggling

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangePainting()
    {
        if (!hasChanged)
        {
            spriteRenderer.sprite = changedPainting; // Switch to creepy version
            hasChanged = true;
            Invoke("ResetPainting", 3f); // Reset after 3 seconds
        }
    }

    void ResetPainting()
    {
        spriteRenderer.sprite = originalPainting; // Switch back
        hasChanged = false;
    }
}
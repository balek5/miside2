// RemoteInteract.cs
using UnityEngine;

public class RemoteInteract : MonoBehaviour
{
    public GameObject tvCanvas;
    public GameObject interactionPrompt; // <-- NEW (this is your "Press E" Canvas)

    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;

    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;
        interactionPrompt.SetActive(false); // Make sure it’s hidden at start
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                interactionPrompt.SetActive(true); // SHOW "Press E"

                if (Input.GetKeyDown(interactionKey))
                {
                    ActivateTV();
                }
            }
            else
            {
                interactionPrompt.SetActive(false); // HIDE if looking somewhere else
            }
        }
        else
        {
            interactionPrompt.SetActive(false); // HIDE if looking far away
        }
    }

    void ActivateTV()
    {
        tvCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float rotationSpeed = 2.0f;
    public float maxLookAngle = 80.0f;
    public Transform cameraTransform;
    public GameObject canvas;
    public Text interactionText;
    private Rigidbody rb;
    private Animator animator;
    private float verticalRotation = 0.0f;
    private bool isAwake = false;
    private bool isInteractingWithPC = false;
    private bool isInteractingWithNPC = false;
    private PlayableDirector timelineDirector;
    public static FirstPersonController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned!");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        rb.isKinematic = false;
        isAwake = false;
        timelineDirector = FindObjectOfType<PlayableDirector>();
        if (timelineDirector != null)
        {
            timelineDirector.stopped += OnTimelineFinished;
        }
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandleMouseLook();
        if (!isAwake)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WakeUp();
            }
            return;
        }
        if (!isInteractingWithPC)
        {
            HandleMovement();
        }
        if (isInteractingWithNPC && Input.GetKeyDown(KeyCode.E))
        {
            StartNPCDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePCInteraction();
        }
        if ((isInteractingWithPC || isInteractingWithNPC) && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePCInteraction();
            ToggleNPCInteraction(false);
        }
    }

    private void WakeUp()
    {
        isAwake = true;
        FindObjectOfType<EnemyController>()?.StartDialogue();
    }

    private void TogglePCInteraction()
    {
        isInteractingWithPC = !isInteractingWithPC;
        if (canvas != null)
        {
            canvas.SetActive(isInteractingWithPC);
        }
        Cursor.lockState = isInteractingWithPC ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInteractingWithPC;
    }

    public void ToggleNPCInteraction(bool state)
    {
        isInteractingWithNPC = state;
        Cursor.lockState = isInteractingWithNPC ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInteractingWithNPC;
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(state);
        }
    }

    private void HandleMouseLook()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(0, horizontalRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * rotationSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleMovement()
    {
        if (isInteractingWithPC) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(horizontal, 0, vertical);
        float inputMagnitude = inputVector.magnitude;
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        direction = direction.normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        float currentSpeed = inputMagnitude * speed;
        UpdateAnimatorParameters(currentSpeed);
        Vector3 targetPosition = rb.position + direction * speed * Time.deltaTime;
        rb.MovePosition(targetPosition);
    }

    private void UpdateAnimatorParameters(float movementSpeed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", movementSpeed);
            animator.SetBool("IsRunning", movementSpeed > 0.8f);
        }
        else
        {
            Debug.LogError("Animator component is missing!");
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        isAwake = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void StartNPCDialogue()
    {
        bool[] isChoiceInteractable = new bool[2];
        for (int i = 0; i < isChoiceInteractable.Length; i++)
        {
            isChoiceInteractable[i] = true;
        }
        DialogueManager.Instance.StartDialogue(new string[0], new string[0], isChoiceInteractable, new string[0], OnChoiceSelected);
        isInteractingWithNPC = false;
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
    }

    public void CloseNPCDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.CloseDialogue(); // Call the DialogueManager's CloseDialogue method
        }
        else
        {
            Debug.LogError("DialogueManager instance is null!");
        }

        isInteractingWithNPC = false;
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
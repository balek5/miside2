using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPromptPanel;
    public GameObject wasdTutorialPanel;
    public GameObject mouseTutorialPanel;
    public Slider keyProgressBar;
    public Slider mouseProgressBar;
    public bool isTutorialComplete = false;
    public Text progressText;
    public Text mouseProgressText;
    public GameObject phonePromptPanel;
    public float phonePromptDuration = 5f;
    public GameObject earlyAccessWarningText;
    public GameObject finalMessagePanel;

    [Header("Tutorial Settings")]
    public int requiredKeyPresses = 10;
    public float requiredMouseMovement = 500f;

    [SerializeField] private FirstPersonController playerController;

    private int currentKeyPresses = 0;
    private float currentMouseMovement = 0f;
    private bool isWASDTutorialActive = false;
    private bool isMouseTutorialActive = false;
    private bool hasShownPhonePrompt = false;

    void Start()
    {
        if (finalMessagePanel == null)
        {
            Debug.LogError("FinalMessagePanel is not assigned in the Inspector.");
        }
        else if (finalMessagePanel.GetComponent<CanvasGroup>() == null)
        {
            Debug.LogError("CanvasGroup is missing on FinalMessagePanel.");
        }
        else
        {
            Debug.Log("FinalMessagePanel and CanvasGroup are properly set up.");
        }

        // FIX: Initialize warning text properly
        if (earlyAccessWarningText != null)
        {
            earlyAccessWarningText.SetActive(false);
        }

        PhoneManager phoneManager = FindObjectOfType<PhoneManager>();
        if (phoneManager != null) phoneManager.enabled = false;

        InitializeTutorial();
    }

    void InitializeTutorial()
    {
        tutorialPromptPanel.SetActive(true);
        wasdTutorialPanel.SetActive(false);
        mouseTutorialPanel.SetActive(false);
        
        keyProgressBar.gameObject.SetActive(false);
        keyProgressBar.maxValue = requiredKeyPresses;
        
        mouseProgressBar.gameObject.SetActive(false);
        mouseProgressBar.maxValue = requiredMouseMovement;
    }

    void Update()
    {
        // FIX: Improved Tab key detection at the start of Update
        if (!isTutorialComplete && Input.GetKeyDown(KeyCode.Tab))
        {
            ShowEarlyAccessWarning();
        }

        if (isWASDTutorialActive)
        {
            TrackKeyPresses();
        }

        if (isMouseTutorialActive)
        {
            TrackMouseMovement();
        }
    }

    // FIX: New dedicated method for showing warning
    void ShowEarlyAccessWarning()
    {
        if (earlyAccessWarningText != null && !earlyAccessWarningText.activeSelf)
        {
            Debug.Log("Early Tab press detected - showing warning");
            StopCoroutine("HideWarningAfterDelay"); // Stop any existing coroutine
            earlyAccessWarningText.SetActive(true);
            StartCoroutine(HideWarningAfterDelay(2f));
        }
    }

    // FIX: Simplified warning coroutine
    IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (earlyAccessWarningText != null)
        {
            earlyAccessWarningText.SetActive(false);
        }
    }

    // --- [Keep all your existing methods exactly the same below this line] ---
    void TrackKeyPresses()
    {
        foreach (KeyCode key in new List<KeyCode> { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D })
        {
            if (Input.GetKeyDown(key))
            {
                currentKeyPresses++;
                UpdateProgressUI();
                break;
            }
        }

        if (currentKeyPresses >= requiredKeyPresses)
        {
            FinishWASDTutorial();
        }
    }

    void FinishWASDTutorial()
    {
        Debug.Log("WASD Tutorial Finished");
        wasdTutorialPanel.SetActive(false);
        keyProgressBar.gameObject.SetActive(false);
        isWASDTutorialActive = false;
        StartMouseTutorial();
    }

    void UpdateProgressUI()
    {
        keyProgressBar.value = currentKeyPresses;
        if (progressText != null)
        {
            progressText.text = $"{currentKeyPresses}/{requiredKeyPresses}";
        }
    }

    void StartMouseTutorial()
    {
        Debug.Log("Mouse Tutorial Started");
        mouseTutorialPanel.SetActive(true);
        mouseProgressBar.gameObject.SetActive(true);
        currentMouseMovement = 0f;
        isMouseTutorialActive = true;
    }

    void TrackMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float delta = new Vector2(mouseX, mouseY).magnitude;
        currentMouseMovement += delta;

        mouseProgressBar.value = currentMouseMovement;

        if (mouseProgressText != null)
        {
            mouseProgressText.text = $"{Mathf.FloorToInt(currentMouseMovement)}/{Mathf.FloorToInt(requiredMouseMovement)}";
        }

        if (currentMouseMovement >= requiredMouseMovement)
        {
            FinishMouseTutorial();
        }
    }

    void FinishMouseTutorial()
    {
        Debug.Log("Mouse Tutorial Finished");
        mouseTutorialPanel.SetActive(false);
        mouseProgressBar.gameObject.SetActive(false);
        isMouseTutorialActive = false;
        isTutorialComplete = true;

        playerController.enabled = true;
        finalMessagePanel.SetActive(true);

        PhoneManager phoneManager = FindObjectOfType<PhoneManager>();
        if (phoneManager != null) phoneManager.enabled = true;

        StartCoroutine(FadeOutCongratsMessage());
        if (!hasShownPhonePrompt) StartCoroutine(ShowPhonePrompt());
    }

    IEnumerator FadeOutCongratsMessage()
    {
        float timeToFade = 2f;
        CanvasGroup canvasGroup = finalMessagePanel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is missing on FinalMessagePanel.");
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < timeToFade)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        finalMessagePanel.SetActive(false);
    }

    IEnumerator ShowPhonePrompt()
    {
        hasShownPhonePrompt = true;
        yield return new WaitForSeconds(2f);
    
        if (phonePromptPanel != null)
        {
            phonePromptPanel.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));
            phonePromptPanel.SetActive(false);
            
            PhoneManager phoneManager = FindObjectOfType<PhoneManager>();
            if (phoneManager != null) phoneManager.OpenPhonePermanently();
        }
    }

    public void OnYesClicked()
    {
        Debug.Log("Tutorial Started");
        tutorialPromptPanel.SetActive(false);
        wasdTutorialPanel.SetActive(true);
        keyProgressBar.gameObject.SetActive(true);
        isWASDTutorialActive = true;
    }

    public void OnNoClicked()
    {
        Debug.Log("Tutorial skipped.");

        // Hide tutorial prompt
        tutorialPromptPanel.SetActive(false);

        // Allow player to move
        playerController.enabled = true;

        // Mark tutorial as complete
        isTutorialComplete = true;

        // Enable phone features
        PhoneManager phoneManager = FindObjectOfType<PhoneManager>();
        if (phoneManager != null)
        {
            phoneManager.enabled = true;
        }

        // Optionally show the phone prompt still
        StartCoroutine(ShowPhonePrompt());
    }
}
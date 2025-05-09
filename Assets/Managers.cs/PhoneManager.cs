using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhoneManager : MonoBehaviour
{
    [Header("Tutorial Lock")]
    public TutorialManager tutorialManager; // Assign in Inspector
    public GameObject earlyAccessWarningText; // Assign your warning UI Text

    [Header("Phone Settings")]
    public GameObject phoneCanvas;
    public KeyCode toggleKey = KeyCode.Tab;

    private bool hasOpenedPhone = false;

    void Start()
    {
        phoneCanvas.SetActive(false);
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    IEnumerator ShowEarlyAccessWarning()
    {
        if (earlyAccessWarningText != null)
        {
            earlyAccessWarningText.SetActive(true);
            yield return new WaitForSeconds(2f);
            earlyAccessWarningText.SetActive(false);
        }
    }

    void Update()
    {
        // Only check TAB input if TutorialManager exists
        if (tutorialManager == null) return;

        if (Input.GetKeyDown(toggleKey))
        {
            // BLOCK if tutorial isn't done
            if (!tutorialManager.isTutorialComplete)
            {
                StartCoroutine(ShowEarlyAccessWarning());
                return; // Exit early
            }

            // Allow phone open if tutorial is done (first time only)
            if (!hasOpenedPhone)
            {
                OpenPhonePermanently();
            }
        }
    }

    public void OpenPhonePermanently()
    {
        hasOpenedPhone = true;
        phoneCanvas.SetActive(true);
        // (Your existing permanent phone logic here)
    }
}
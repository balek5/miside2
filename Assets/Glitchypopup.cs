using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlitchyPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public Text popupText;
    public Button yesButton;
    public Button noButton;

    private void Start()
    {
        popupPanel.SetActive(false); // Initially hide the popup
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    // Call this method to show the popup
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        StartCoroutine(GlitchEffect());
    }

    // Hide the popup
    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    // Glitch effect coroutine
    private IEnumerator GlitchEffect()
    {
        float glitchDuration = 1f; // Duration of the glitch effect
        float timeElapsed = 0f;

        while (timeElapsed < glitchDuration)
        {
            float glitchAmount = Random.Range(0.02f, 0.1f);
            Vector3 originalPosition = popupPanel.transform.position;
            popupPanel.transform.position = new Vector3(originalPosition.x + Random.Range(-glitchAmount, glitchAmount),
                                                        originalPosition.y + Random.Range(-glitchAmount, glitchAmount),
                                                        originalPosition.z);

            // Randomize the text color for a glitchy look
            popupText.color = new Color(Random.value, Random.value, Random.value);

            // Apply a random scale for the popup
            float scaleAmount = Random.Range(0.95f, 1.05f);
            popupPanel.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1f);

            yield return new WaitForSeconds(0.05f); // Delay for each glitch frame
            timeElapsed += 0.05f;
        }

        // Reset the popup to its original position, color, and scale after glitching
        popupPanel.transform.position = new Vector3(0, 0, 0);
        popupText.color = Color.white;
        popupPanel.transform.localScale = Vector3.one;
    }

    // On "Yes" button click
    private void OnYesClicked()
    {
        Debug.Log("Confirmed!");
        HidePopup();
    }

    // On "No" button click
    private void OnNoClicked()
    {
        Debug.Log("Cancelled!");
        HidePopup();
    }
}

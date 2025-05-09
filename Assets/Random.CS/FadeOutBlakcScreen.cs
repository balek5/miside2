using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI storyText; // UI Text for the story
    public Image fadeImage; // Full-screen black UI Image
    public float typingSpeed = 0.05f; // Speed of text animation
    public float fadeDuration = 3f; // Total time for fade
    public float textFadeStartTime = 1f; // Time before fade ends when text should start disappearing

    private string fullStory = "After a long day, you have finally got home ...\nExcited to download SeeingMio.";

    void Start()
    {
        // Start with a black screen
        fadeImage.color = new Color(0, 0, 0, 1);
        storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, 1); // Make text fully visible

        // Begin the wake-up sequence
        StartCoroutine(WakeUpSequence());
    }

    IEnumerator WakeUpSequence()
    {
        // Step 1: Type out the text while the screen is black
        yield return StartCoroutine(TypeText(fullStory));

        // Step 2: Wait briefly before starting the fade-in
        yield return new WaitForSeconds(0.5f);

        // Step 3: Start both the screen fade-in and text fade-out
        StartCoroutine(FadeOutText());
        yield return StartCoroutine(FadeToNormal());

        // Step 4: Disable text completely after fade-out
        storyText.gameObject.SetActive(false);
    }

    IEnumerator TypeText(string text)
    {
        storyText.text = ""; // Clear text first
        foreach (char letter in text.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeToNormal()
    {
        float elapsedTime = 0f;
        Color fadeColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 0;
        fadeImage.color = fadeColor;
    }

    IEnumerator FadeOutText()
    {
        float textFadeDuration = fadeDuration - textFadeStartTime; // Text should finish fading before the screen fade ends
        float elapsedTime = 0f;
        Color textColor = storyText.color;

        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, elapsedTime / textFadeDuration);
            storyText.color = textColor;
            yield return null;
        }

        textColor.a = 0;
        storyText.color = textColor;
    }
}

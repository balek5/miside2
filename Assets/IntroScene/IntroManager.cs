using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    public GameObject nameInputPanel;
    public TMP_InputField nameInputField;

    [Header("Character Name")]
    public string characterName = "Elara";

    public GameObject characterModel;
    public Animator characterAnimator;

    [Header("Loading Screen")]
    public GameObject loadingScreenCanvas;
    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public Slider loadingSlider;
    public float loadingTime = 5f; // You can adjust this as needed
    public string sceneToLoad = "MainGame"; // Replace with your scene name

    private void Start()
    {
        if (characterModel != null) characterModel.SetActive(true);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (nameInputPanel != null) nameInputPanel.SetActive(false);

        if (loadingScreen != null) loadingScreen.SetActive(false);
        if (loadingScreenCanvas != null) loadingScreenCanvas.SetActive(false);

        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        yield return ShowDialogue($"Hi, I'm {characterName}.");
        yield return new WaitForSeconds(1.5f);

        yield return ShowDialogue("Before we begin, what's your name?");

        if (nameInputPanel != null) nameInputPanel.SetActive(true);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public void OnNameConfirmed()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("No name entered.");
            return;
        }

        if (nameInputPanel != null) nameInputPanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        StartCoroutine(RespondToName(playerName));
    }

    IEnumerator RespondToName(string playerName)
    {
        yield return ShowDialogue($"Nice to meet you, {playerName}.");
        yield return new WaitForSeconds(1f);

        // Show loading screen and begin simulated loading
        if (loadingScreenCanvas != null) loadingScreenCanvas.SetActive(true);
        StartCoroutine(LoadGameWithFixedTime());
    }

    IEnumerator LoadGameWithFixedTime()
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            float progress = Mathf.Clamp01(elapsedTime / loadingTime);

            if (loadingSlider != null)
                loadingSlider.value = progress;

            if (loadingText != null)
                loadingText.text = "Loading... " + Mathf.FloorToInt(progress * 100) + "%";

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (loadingText != null)
            loadingText.text = "Loading Complete!";

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneToLoad); // Load your game scene
    }

    IEnumerator ShowDialogue(string message)
    {
        if (characterAnimator != null)
            characterAnimator.SetBool("IsTalking", true);

        if (dialogueText != null)
        {
            dialogueText.text = "";
            foreach (char c in message)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(0.03f);
            }
        }

        if (characterAnimator != null)
            characterAnimator.SetBool("IsTalking", false);
    }
}

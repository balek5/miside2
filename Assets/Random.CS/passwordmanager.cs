using UnityEngine;
using UnityEngine.UI; // For UI components
using UnityEngine.SceneManagement; // For scene management

public class DesktopPuzzle : MonoBehaviour
{
    public string correctPassword = "UnityGame"; // Set the correct password
    public InputField passwordInputField; // Reference to the password input field
    public Button submitButton; // Reference to the submit button
    public Text feedbackText; // Reference to the feedback text
    public string nextSceneName = "Desktopscene"; // Name of the next scene to load after solving the puzzle

    private void Start()
    {
        // Ensure InputField is interactable and clear the text
        passwordInputField.text = "";
        passwordInputField.interactable = true;

        // Automatically select the input field when the scene starts
        passwordInputField.Select();
        passwordInputField.ActivateInputField();

        // Add a listener to the submit button to trigger the CheckPassword function
        submitButton.onClick.AddListener(CheckPassword);
    }

    public void CheckPassword()
    {
        string enteredPassword = passwordInputField.text;
        Debug.Log("Entered Password: " + enteredPassword); // Debugging line

        if (enteredPassword == correctPassword)
        {
            Debug.Log("Password is correct. Loading next scene...");
            feedbackText.text = "Access Granted!";
            feedbackText.color = Color.green;
            Invoke("LoadNextScene", 1.5f);
        }
        else
        {
            Debug.Log("Incorrect password!");
            feedbackText.text = "Incorrect Password! Try Again.";
            feedbackText.color = Color.red;
            passwordInputField.text = "";
            passwordInputField.Select();
            passwordInputField.ActivateInputField();
        }
    }


    private void LoadNextScene()
    {
        // Load the next scene (you can change the scene name as needed)
        SceneManager.LoadScene(nextSceneName);
    }
}

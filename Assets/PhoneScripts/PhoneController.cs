using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject phoneUI; // Assign in inspector
    public Animator phoneAnimator; // Assign the PhoneContainer's Animator
    public AudioSource phoneAudio; // Assign your audio source

    private bool isPhoneOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePhone();
        }
    }

    public void TogglePhone()
    {
        isPhoneOpen = !isPhoneOpen;

        if (isPhoneOpen)
        {
            phoneUI.SetActive(true);
            phoneAnimator.SetTrigger("Open");
            phoneAudio.Play();
        }
        else
        {
            phoneAnimator.SetTrigger("Close");
            // Optional: Add delay before deactivating matching animation length
            StartCoroutine(DeactivateAfterAnimation());
        }
    }

    private System.Collections.IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Match your animation duration
        phoneUI.SetActive(false);
    }
}
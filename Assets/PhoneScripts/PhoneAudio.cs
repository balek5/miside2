using UnityEngine;

public class PhoneAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip interactSound;
    public AudioClip downloadSound;
    public AudioClip completeSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayInteractSound()
    {
        audioSource.PlayOneShot(interactSound);
    }

    public void PlayDownloadSound()
    {
        audioSource.PlayOneShot(downloadSound);
    }

    public void PlayCompleteSound()
    {
        audioSource.PlayOneShot(completeSound);
    }
}
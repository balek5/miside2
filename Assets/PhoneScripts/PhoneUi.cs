using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;


public class PhoneUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject phoneScreen; // Reference to the phone screen Canvas
    public Image progressBarFill;
    public TextMeshProUGUI statusText;
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;

    [Header("Settings")]
    public float downloadTime = 3f;
    private bool isDownloading = false;

    void Start()
    {
        if (phoneScreen != null)
        {
            phoneScreen.SetActive(false); // Hide the phone screen initially
        }
    }

    public void StartDownload()
    {
        if (!isDownloading)
        {
            isDownloading = true;
            if (phoneScreen != null)
            {
                phoneScreen.SetActive(true); // Show the phone screen
            }
            StartCoroutine(DownloadProgress());
        }
    }

    System.Collections.IEnumerator DownloadProgress()
    {
        float timer = 0;
        while (timer < downloadTime)
        {
            timer += Time.deltaTime;
            float progress = timer / downloadTime;
            UpdateProgress(progress);
            yield return null;
        }
        CompleteDownload();
        isDownloading = false;
    }

    void UpdateProgress(float progress)
    {
        progressBarFill.fillAmount = progress;
        statusText.text = $"DOWNLOADING... {(int)(progress * 100)}%";
    }

    void CompleteDownload()
    {
        notificationText.text = "APP INSTALLED!";
        ShowNotification();
    }

    void ShowNotification()
    {
        notificationPanel.SetActive(true);
        Invoke("HideNotification", 2f);
    }

    void HideNotification()
    {
        notificationPanel.SetActive(false);
        if (phoneScreen != null)
        {
            phoneScreen.SetActive(false); // Hide the phone screen after download
        }
    }
}
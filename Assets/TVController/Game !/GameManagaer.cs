using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioClip streakBreakSound;
    public TMP_Text scoreText;
    public AudioClip hitSound;
    public AudioClip missSound;
    public AudioClip[] comboSounds; // array of 10 sounds for combo milestones 2–11
    private int combo = 0;
    private AudioSource audioSource;
    private int score = 0;
    public Text comboText;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void IncreaseScore()
    {
        score += 100;
        combo++;

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (comboText != null)
            comboText.text = "Combo: " + combo;

        PlayHitSound();
        PlayComboSound(combo);
    }


    public void PlayHitSound()
    {
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    public void PlayMissSound()
    {
        if (missSound != null)
            audioSource.PlayOneShot(missSound);

        if (combo >= 2 && streakBreakSound != null)
        {
            // Only play if you had a streak
            audioSource.PlayOneShot(streakBreakSound);
        }

        combo = 0;

        if (comboText != null)
            comboText.text = "Combo: 0";
    }


    public void PlayComboSound(int currentCombo)
    {
        // Play combo sound only from combo 2 to 11 (index 0 to 9)
        if (currentCombo >= 2 && currentCombo <= 11)
        {
            int index = currentCombo - 2; // combo 2 = index 0
            if (comboSounds.Length > index && comboSounds[index] != null)
            {
                audioSource.PlayOneShot(comboSounds[index]);
            }
        }
    }
    
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Slider))]
public class DynamicSliderFill : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI percentageText; // Fixed declaration
    public Color inactiveColor = new Color(1, 1, 1, 0);
    public Color activeColor = Color.white;

    public float sensitivity = 0.3f; // Lower = slower movement
    private Slider slider;
    private float targetValue;
    private CanvasGroup fillCanvasGroup;
    private bool wasActive;

    void Start()
    {
        slider = GetComponent<Slider>();
        targetValue = slider.value;

        if (fillImage != null)
        {
            // Get or add CanvasGroup component
            fillCanvasGroup = fillImage.gameObject.GetComponent<CanvasGroup>();
            if (fillCanvasGroup == null)
            {
                fillCanvasGroup = fillImage.gameObject.AddComponent<CanvasGroup>();
            }

            fillImage.color = inactiveColor;
            fillImage.canvasRenderer.SetAlpha(0f);
        }

        if (slider.value == 0f && fillCanvasGroup != null)
        {
            fillCanvasGroup.alpha = 0f;
        }

        wasActive = slider.value > 0;

        if (percentageText != null)
        {
            percentageText.text = $"{Mathf.RoundToInt(slider.value * 100)}%";
        }

        // Hook into onValueChanged but don't update directly
        slider.onValueChanged.AddListener(OnSliderRawValueChanged);
    }

    void OnSliderRawValueChanged(float value)
    {
        targetValue = value;
    }

    void Update()
    {
        // Smoothly interpolate toward the target value
        slider.SetValueWithoutNotify(Mathf.Lerp(slider.value, targetValue, Time.deltaTime / sensitivity));

        if (percentageText != null)
        {
            percentageText.text = $"{Mathf.RoundToInt(slider.value * 100)}%";
        }

        // Only trigger animations when the active state changes
        bool isActive = slider.value > 0;
        if (isActive != wasActive && fillImage != null)
        {
            wasActive = isActive;
            float alpha = isActive ? 1f : 0f;

            // Use coroutines instead of LeanTween
            StartCoroutine(FadeCanvasGroup(fillCanvasGroup, alpha, 0.3f));
            StartCoroutine(AnimateColor(isActive ? activeColor : inactiveColor, 0.5f, 0.1f));
        }
    }

    // Replacement for LeanTween.alpha
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float targetAlpha, float duration)
    {
        float startAlpha = cg.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        cg.alpha = targetAlpha;
    }

    // Replacement for LeanTween.value
    private IEnumerator AnimateColor(Color targetColor, float duration, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        Color startColor = fillImage.color;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            fillImage.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        fillImage.color = targetColor;
    }
}
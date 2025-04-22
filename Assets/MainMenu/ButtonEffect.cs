using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 0.95f;
    [SerializeField] private Color hoverColor = Color.cyan;

    private Vector3 originalScale;
    private Color originalColor;
    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalScale = transform.localScale;
        originalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        buttonImage.color = originalColor;
    }

    public void OnPointerDown()
    {
        transform.localScale = originalScale * clickScale;
    }

    public void OnPointerUp()
    {
        transform.localScale = originalScale * hoverScale;
    }
}
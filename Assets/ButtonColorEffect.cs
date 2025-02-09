using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonColorEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public Color clickColor = Color.green;

    public float hoverScaleFactor = 1.2f;  // The scale factor when hovering
    public float hoverSpeed = 0.1f;  // Speed of scaling up and down
    public float quitButtonMoveSpeed = 2f;  // Speed at which the quit button moves away

    private Image buttonImage;  // Reference to the button's Image component
    private RectTransform buttonRectTransform;  // Reference to the button's RectTransform (for scaling and moving)

    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        // Get the button's Image component
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("Button Image component not found on this GameObject.");
        }

        // Get the RectTransform for scaling and positioning
        buttonRectTransform = GetComponent<RectTransform>();
        if (buttonRectTransform == null)
        {
            Debug.LogError("Button RectTransform not found on this GameObject.");
        }

        // Store the original scale and position for later use
        if (buttonRectTransform != null)
        {
            originalScale = buttonRectTransform.localScale;
            originalPosition = buttonRectTransform.localPosition;
        }

        // Set the button's color to normal color at the start
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }

        // Scale up the button on hover
        if (buttonRectTransform != null)
        {
            StopAllCoroutines();  // Stop any scaling effects
            StartCoroutine(ScaleButtonUp());  // Start the scaling up effect
        }

        Debug.Log("Mouse entered the button area.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }

        // Reset the button scale when the mouse exits
        if (buttonRectTransform != null)
        {
            StopAllCoroutines();  // Stop any scaling effects
            StartCoroutine(ScaleButtonDown());  // Start scaling down effect
        }

        Debug.Log("Mouse exited the button area.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = clickColor;
        }

        // Handle quit button logic if it's the quit button
        if (gameObject.name == "QuitButton")
        {
            StartCoroutine(MoveQuitButton());
        }

        Debug.Log("Button clicked!");
    }

    private IEnumerator ScaleButtonUp()
    {
        // Scale the button up slowly when hovered over
        while (buttonRectTransform.localScale.x < originalScale.x * hoverScaleFactor)
        {
            buttonRectTransform.localScale += new Vector3(hoverSpeed, hoverSpeed, 0);
            yield return null;
        }
    }

    private IEnumerator ScaleButtonDown()
    {
        // Scale the button back to its original size when not hovered over
        while (buttonRectTransform.localScale.x > originalScale.x)
        {
            buttonRectTransform.localScale -= new Vector3(hoverSpeed, hoverSpeed, 0);
            yield return null;
        }
    }

    private IEnumerator MoveQuitButton()
    {
        // Continuously move the quit button away from the cursor
        Vector3 targetPosition = originalPosition + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);

        while (Vector3.Distance(buttonRectTransform.localPosition, targetPosition) > 1f)
        {
            buttonRectTransform.localPosition = Vector3.MoveTowards(buttonRectTransform.localPosition, targetPosition, quitButtonMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

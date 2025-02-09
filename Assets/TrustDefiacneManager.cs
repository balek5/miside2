using UnityEngine;
using UnityEngine.UI;

public class TrustDefianceManager : MonoBehaviour
{
    public Slider trustBar;   // Reference to Trust UI Bar
    public Slider defianceBar; // Reference to Defiance UI Bar

    private int trustValue = 0;   // Default Neutral Value
    private int defianceValue = 0;

    void Start()
    {
        UpdateBars();
    }

    public void ChangeTrust(int amount)
    {
        trustValue = Mathf.Clamp(trustValue + amount, 0, 100);
        defianceValue = Mathf.Clamp(defianceValue - amount, 0, 100);
        UpdateBars();
    }

    public void ChangeDefiance(int amount)
    {
        defianceValue = Mathf.Clamp(defianceValue + amount, 0, 100);
        trustValue = Mathf.Clamp(trustValue - amount, 0, 100);
        UpdateBars();
    }

    void UpdateBars()
    {
        trustBar.value = trustValue;
        defianceBar.value = defianceValue;
    }
}
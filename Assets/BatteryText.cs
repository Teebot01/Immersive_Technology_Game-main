using UnityEngine;
using TMPro;

public class BatteryText : MonoBehaviour
{
    private TextMeshProUGUI thisText; // Use TextMeshProUGUI for UI text
    public LightPower powerVariables;

    void Start()
    {
        thisText = GetComponent<TextMeshProUGUI>(); // Ensure it's a UI TextMeshPro
        if (thisText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on GameObject!");
        }

        if (powerVariables == null)
        {
            Debug.LogError("LightPower script reference is missing in BatteryText!");
        }
    }

    void Update()
    {
        if (thisText != null && powerVariables != null && powerVariables.thisLight != null)
        {
            float intensity = powerVariables.thisLight.intensity;
            float maxIntensity = powerVariables.maxIntensity;
            float batteryPercentage = (intensity / maxIntensity) * 100f; // Convert to percentage

            thisText.text = "Battery Power: " + batteryPercentage.ToString("F0") + "%"; // Display as whole number percentage
        }
    }
}


using UnityEngine;
using TMPro;

public class BatteryText : MonoBehaviour
{
    private TextMeshProUGUI thisBatteryText; // Use TextMeshProUGUI for UI text
    public LightPower powerVariables;

    void Start()
    {
        thisBatteryText = GetComponent<TextMeshProUGUI>(); // Ensure it's a UI TextMeshPro
        if (thisBatteryText == null)
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
        if (thisBatteryText != null && powerVariables != null && powerVariables.thisLight != null)
        {
            float intensity = powerVariables.thisLight.intensity;
            float maxIntensity = powerVariables.maxIntensity;
            float batteryPercentage = (intensity / maxIntensity) * 100f; // Convert to percentage

            thisBatteryText.text = "Battery Power: " + batteryPercentage.ToString("F0") + "%"; // Display as whole number percentage
        }
    }
}


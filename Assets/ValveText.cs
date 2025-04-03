using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValveText : MonoBehaviour
{
    private TextMeshProUGUI thisValveText;
    public ValveManager valvesManager;

    void Start()
    {
        thisValveText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Using string interpolation (Best practice)
        thisValveText.text = $"Valves Closed: {valvesManager.closedValveCount}/{valvesManager.closedValveRequirement}";
    }
}


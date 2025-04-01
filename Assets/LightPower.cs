using System.Collections;
using UnityEngine;

public class LightPower : MonoBehaviour
{
    private bool isRunning = true;
    public float powerReduction = 0.2f;
    public float powerIncrease = 0.5f;
    public float reductionInterval = 3.0f; // Time between reductions
    private Light thisLight;

    void Start()
    {
        thisLight = GetComponent<Light>();
        if (thisLight == null)
        {
            Debug.LogError("No Light component found on this GameObject!");
            isRunning = false;
            return;
        }

        if (thisLight.intensity >= 0)
        {
            StartCoroutine(PowerReductionRoutine());
        }
    }

    void Update()
    {
        // Check for player input (e.g., space bar or mouse click)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseLightIntensity();
        }
    }

    IEnumerator PowerReductionRoutine()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(reductionInterval);
            thisLight.intensity = Mathf.Max(0, thisLight.intensity - powerReduction);
            Debug.Log("Power Level After Reduction:" + thisLight.intensity);
        }
    }

    void IncreaseLightIntensity()
    {
        thisLight.intensity += powerIncrease;
    }
}
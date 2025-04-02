using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class LightPower : MonoBehaviour
{
    [SerializeField] private InputActionReference chargeFlashlight_L;
    [SerializeField] private InputActionReference chargeFlashlight_R;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Detect if flashlight is held

    private bool isHeld = false; // Check if flashlight is held
    private bool isRunning = true;

    public float powerReduction = 0.2f;
    public float powerIncrease = 0.5f;
    public float reductionInterval = 3.0f; // Time between reductions
    public float maxIntensity = 3.0f;
    public Light thisLight;

    void Start()
    {
        thisLight = GetComponent<Light>();
        if (thisLight == null)
        {
            Debug.LogError("No Light component found on this GameObject!");
            isRunning = false;
            return;
        }

        // Enable Input Actions
        chargeFlashlight_L.action.Enable();
        chargeFlashlight_R.action.Enable();

        // Subscribe to input events
        chargeFlashlight_L.action.performed += IncreaseLightIntensity;
        chargeFlashlight_R.action.performed += IncreaseLightIntensity;

        // Subscribe to Grab Events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        StartCoroutine(PowerReductionRoutine());
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        chargeFlashlight_L.action.performed -= IncreaseLightIntensity;
        chargeFlashlight_R.action.performed -= IncreaseLightIntensity;

        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
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

    void IncreaseLightIntensity(InputAction.CallbackContext context)
    {
        if (isHeld && thisLight.intensity < maxIntensity) // Set 3.0f as the max intensity limit
        {
            thisLight.intensity += powerIncrease;
            Debug.Log("Power Level After Increase: " + thisLight.intensity);
        }
        else if (thisLight.intensity >= 3.0f)
        {
            Debug.Log("Flashlight is fully charged!");
        }
    }


    // Detect if the flashlight is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    // Detect if the flashlight is released
    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
    }
}

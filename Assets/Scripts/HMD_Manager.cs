using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMD_Manager : MonoBehaviour
{
    [SerializeField] GameObject xrPlayer;
    [SerializeField] GameObject fpsPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Using Device:" + XRSettings.loadedDeviceName);
        if (XRSettings.isDeviceActive || XRSettings.loadedDeviceName == "OpenXR Display")
        {
            Debug.Log("Using device XR Player with HMD:" + XRSettings.loadedDeviceName);
            fpsPlayer.SetActive(false);
            xrPlayer.SetActive(true);
        }
        else
        {
            Debug.Log("No HMD destected, using FPS player");
            xrPlayer.SetActive(false);
            fpsPlayer.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

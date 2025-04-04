using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValveManager : MonoBehaviour
{
    public int closedValveCount = 0;
    public int closedValveRequirement = 5;
    public RawImage proofImage;

    // Update is called once per frame
    void Update()
    {
        if(closedValveCount == closedValveRequirement)
        {
            Debug.Log("All Valves Activated");
            proofImage.enabled = true;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveManager : MonoBehaviour
{
    public int closedValveCount = 0;
    public int closedValveRequirement = 5;

    // Update is called once per frame
    void Update()
    {
        if(closedValveCount == closedValveRequirement)
        {
            Debug.Log("All Valves Activated");
        }
    }
}

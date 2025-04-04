using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlank : MonoBehaviour
{
    public GameObject planksParentObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AxeHead")
        {
            Destroy(this.gameObject);
        }
    }
}

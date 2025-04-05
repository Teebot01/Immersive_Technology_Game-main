using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlank : MonoBehaviour
{
    public GameObject planksParentObject;
    private MeshRenderer thisMeshRenderer;
    private BoxCollider thisBoxCollider;
    private BoxCollider thisTriggerBoxCollider = null;
    public bool isDestroyed = false;
    private AudioSource parentAudioSource;

    private void Start()
    {
        thisMeshRenderer = GetComponent<MeshRenderer>();
        thisBoxCollider = GetComponent<BoxCollider>();
        parentAudioSource = planksParentObject.GetComponent<AudioSource>();

        foreach (BoxCollider box in GetComponents<BoxCollider>())
        {
            if (box.isTrigger)
            {
                thisTriggerBoxCollider = box;
                break; // Optional: if you only want the first one
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AxeHead")
        {
            thisMeshRenderer.enabled = false;
            thisBoxCollider.enabled = false;
            thisTriggerBoxCollider.enabled = false;
            isDestroyed = true;
            parentAudioSource.Play();
        }
    }
}
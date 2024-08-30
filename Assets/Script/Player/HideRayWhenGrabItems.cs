using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideRayWhenGrabItems : MonoBehaviour
{
    private XRInteractorLineVisual ray;
    // Start is called before the first frame update
    void Start()
    {
        ray = GetComponent<XRInteractorLineVisual>();
    }

    public void HideRay()
    {
        ray.enabled = false;
    }
    public void ShowRay()
    {
        ray.enabled = true;
    }
}

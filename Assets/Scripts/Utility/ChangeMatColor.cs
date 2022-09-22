using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    Renderer[] thisRenderer = null;
    
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = this.GetComponentsInChildren<Renderer>();
    }

    public void SetFrameColor(Color colorVal)
    {
        foreach (Renderer cubeRender in thisRenderer)
        {
            cubeRender.material.SetColor("_Color", colorVal);
        }
    }
}

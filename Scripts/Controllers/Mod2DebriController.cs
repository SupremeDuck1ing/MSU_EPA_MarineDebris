//Controller Script for Debris in Module 2, inheritor from debris controller script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod2DebriController : DebriController
{
    Vector3 globalScale;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        globalScale = transform.lossyScale;
    }


    protected override void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "handnet":
                isInNet = true;
                Debug.Log("TRIGGERED ON NET");
                collider.gameObject.GetComponent<NetController>().PlayNetSound();
                GetComponent<Outline>().enabled = false;
                GetComponent<OutlineController>().doDefaultFlash = false;
                transform.SetParent(collider.gameObject.transform.parent);
                transform.position = orientationHandle.position;
                transform.rotation = orientationHandle.rotation;
                 
                break;
            default:
                break;
        }
    }
}

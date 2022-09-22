//Controller Script for Marine Debris

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DebriController : MonoBehaviour
{
    [HideInInspector]
    public Transform orientationHandle;
    protected Collider colliderComp;
    protected Rigidbody rigidComp;
    [HideInInspector]
    public bool isInNet = false;

    //Awake called before Start: References collider and rigidbody of marine debris
    void Awake()
    {
        
        colliderComp = GetComponent<Collider>();
        rigidComp = GetComponent<Rigidbody>();
    }

    //Reference transform position to move debris to when picked up (i.e center of net)
    protected virtual void Start()
    {
        orientationHandle = GameObject.FindGameObjectWithTag("netHandle").transform;
    }

    //Function Triggers when the collider of the marine debris interacts with another colider
    protected virtual void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        { 
            //Specifies that the 2nd collider must be from the handnet 
            //Disables pick-up outline, flips inNet bool, sets debris position to center of net 
            //Also sets debris parent to net, allows debris to move in sync w/ net by simply moving the net parent
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
 
    //Will be called when net is turned upside-down to represent dumping debris, basically resets the changes from OnTriggerEnter() 
    //Note that debris in unparented from net so moving net no longer also moves debris. 
    //!: Parenting & Unparenting, especially multiple times will deform child object (debris) due to inconsistent scaling issues when switching parents
    public void DetachFromNet()
    {
        Physics.IgnoreLayerCollision(12,13,true);
        isInNet = false;
        transform.SetParent(null);
        colliderComp.isTrigger = false;
        rigidComp.useGravity = true;
        rigidComp.isKinematic = false;
    }
}

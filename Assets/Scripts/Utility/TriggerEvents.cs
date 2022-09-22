using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    
    [SerializeField]
    public UnityEvent CallFunctions = null;
    [SerializeField]
    public bool callOnStart = false;
    // Problem:  Collision not triggering on specific object
    // Solution:  The gameObject with the Collider trigger enabled, has to have a rigidbody on the same object
    
    void Start()
    {
        if (callOnStart)
        {
            CallEvents();
        }
    }

    public void CallEvents()
    {
        CallFunctions.Invoke();
    }


}

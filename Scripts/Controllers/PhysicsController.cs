//Controller Script for General Physics Interactions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class PhysicsController : MonoBehaviour
{
    protected Rigidbody rigidbodyComp;
    protected Collider colliderComp;
    protected float originalDrag;
    [HideInInspector]
    public bool stoppingRoll = false;
    [HideInInspector]
    public bool isBeingDestroyed = false;
    protected AudioSource audioSource;
    protected OutlineController outlineController;
    
    protected virtual void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        rigidbodyComp = GetComponent<Rigidbody>();
        colliderComp = GetComponent<Collider>();
        outlineController = GetComponent<OutlineController>();
        if (!colliderComp)
        {
            colliderComp = GetComponentInChildren<Collider>();
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalDrag = rigidbodyComp.angularDrag;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Did we collide with the sand?
        // 9 is teleportable layer (aka ground/sand), 14 is Bin layer
        if (collision.gameObject.layer == 9 && !stoppingRoll && !isBeingDestroyed)
        {
            audioSource.Play();
            StartCoroutine(StopRolling(collision.gameObject.layer));
        } 
        else if (collision.gameObject.layer == 15 && !stoppingRoll && !isBeingDestroyed)
        {
            audioSource.Play();
            StartCoroutine(StopRolling(collision.gameObject.layer));
        } 
    }

    // Begins when the object hits the ground or bin layers and is not currently running this coroutine
    protected virtual IEnumerator StopRolling(int layer)
    {
        stoppingRoll = true;
        Debug.Log("StopRolling()");
        outlineController.doRejectOutline = false;
        // ignore the net while we are stopping the roll.
        Physics.IgnoreLayerCollision(12,13,true);
        rigidbodyComp.angularDrag = originalDrag;
        if (layer == 9)
        {
            while (isMoving())
            {
                rigidbodyComp.angularDrag += Time.deltaTime;
                if (rigidbodyComp.angularDrag > 11.5f)
                {
                    rigidbodyComp.isKinematic = true;
                    colliderComp.isTrigger = true;
                    break;
                }
                yield return null;
            }
        }
        rigidbodyComp.isKinematic = true;
        colliderComp.isTrigger = true;
        rigidbodyComp.angularDrag = originalDrag;
        Physics.IgnoreLayerCollision(12,13,false);
        outlineController.StartDefaultFlash();
        Debug.Log(transform.position);
        Debug.Log("Exiting StopRolling()");
        stoppingRoll = false;
    }

    IEnumerator DebugPhysics()
    {
        while (stoppingRoll)
        {
            yield return new WaitForSeconds(0.25f);
            Debug.Log(rigidbodyComp.velocity);
        }
    }

    //Check Functions for current velocity & angular velocity

    protected bool isMoving()
    {
        return rigidbodyComp.velocity.normalized.y != 0;
    }

    bool hasAngularVelocity()
    {
        return rigidbodyComp.angularVelocity.normalized != new Vector3(0,0,0);
    }


}

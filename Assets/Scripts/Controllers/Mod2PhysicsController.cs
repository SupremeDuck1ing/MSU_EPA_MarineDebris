//Controller Script for the physics in Module 2, inheritor from physics controller script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod2PhysicsController : PhysicsController
{
    protected Vector3 originalPos; 
    protected Vector3 originalRotation;
    public AudioClip[] clips;
    protected override void Start()
    {
        base.Start();
        originalPos = transform.position;
        originalRotation = transform.eulerAngles;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        // Did we collide with the sand?
        // 9 is teleportable layer (aka ground/sand), 14 is Bin layer, 4 is water layer I believe
        if (collision.gameObject.layer == 9 && !stoppingRoll && !isBeingDestroyed)
        {
            StartCoroutine(StopRolling(collision.gameObject.layer));
        }
        else if (collision.gameObject.layer == 4 && !stoppingRoll && !isBeingDestroyed)
        {
            StartCoroutine(StopRolling(collision.gameObject.layer));
        }
    }

    protected override IEnumerator StopRolling(int layer)
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
                    transform.position = originalPos;
                    transform.eulerAngles = originalRotation;
                    break;
                }
                yield return null;
            }
            rigidbodyComp.angularDrag = originalDrag;
            Physics.IgnoreLayerCollision(12,13,false);
        }
        // water layer
        if (layer == 4)
        {
            yield return new WaitForSeconds(2.5f);
            rigidbodyComp.isKinematic = true;
            colliderComp.isTrigger = true;
            transform.position = originalPos;
            transform.eulerAngles = originalRotation;
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
}

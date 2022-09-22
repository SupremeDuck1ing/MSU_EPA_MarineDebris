//Controller Script for interactable picture frames that interact w/ npc in mod 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{

    public void DetachFromNPC()
    {
        // only try to detach if we are currently attached
        if (transform.parent.tag == "NPC_Hand")
        {
            // update state in AnimDriver script on _MainRig gameboject
            Transform t = transform;
            // traverse upward in the hierarchy until we get to the NPC tagged gameobject
            while (t.parent != null)
            {
                if (t.parent.tag == "NPC")
                {
                    t = t.parent.transform;
                    break;
                }
                t = t.parent.transform;
            }
            // Now that we have reference to _MainRig, we update the state of the give behavior
            t.GetComponent<AnimDriver>().anim.GetBehaviour<GiveBehavior>().itemGrabbed = true;
            // this interactable gameobject no longer will have a parent
            transform.parent = null;
        }
    }

    public void DisableInteractableAfterGrab(GameObject objectToDisable)
    {
        StartCoroutine(DisableInteractableAfterGrabRoutine(objectToDisable));
    }

    public IEnumerator DisableInteractableAfterGrabRoutine(GameObject objectToDisable)
    {
        // this should resolve a race condition where the hands break when the game object is disabled in the hand.
        yield return new WaitForEndOfFrame();
        objectToDisable.SetActive(false);
    }
}

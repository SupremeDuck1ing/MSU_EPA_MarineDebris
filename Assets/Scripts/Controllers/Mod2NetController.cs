//Controller script for the net in module 2, inheritor of the net controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod2NetController : NetController
{
    protected override void UpdateOrientationState()
    {
        // check if there is debri in the net
        DebriController debriControllerComp = gameObject.transform.parent.gameObject.GetComponentInChildren<DebriController>();
        if (debriControllerComp)
        {
            //.parent.parent.parent for gameobject that is actually being rotated (has interactableFacade)
            GameObject interactableObject = gameObject.transform.parent.parent.parent.gameObject;
            // Module 2 net has all rotation values set to 0, so it will behave differently from module 3's net controller.
            if (interactableObject.transform.rotation.eulerAngles.z > -30 && interactableObject.transform.eulerAngles.z < 30)
            {
                audioSource.PlayOneShot(GetRandomClip());
                debriControllerComp.DetachFromNet();
            }
        }
    }
}

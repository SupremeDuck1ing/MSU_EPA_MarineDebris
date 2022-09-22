using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.PointerInteractors;
using Tilia.Interactions.Interactables.Interactables;
using Zinnia.Utility;
using Zinnia.Event.Proxy;

public class VRTKDriver : MonoBehaviour
{
    public GameObject distanceGrabber;
    public static VRTKDriver instance;

    void Awake()
    {
        instance = this;
        distanceGrabber = GameObject.FindGameObjectWithTag("DistanceGrabber");
    }

    void Start()
    {
        
    }

    public void ClearFirstGrabbedEvents(InteractableFacade interactableFacade)
    {
        StartCoroutine(ClearFirstGrabbedEventsRoutine(interactableFacade));
    }
    IEnumerator ClearFirstGrabbedEventsRoutine(InteractableFacade interactableFacade)
    {
        if (interactableFacade == null)
        {
            Debug.LogError("No interactable object given for ClearFirstGrabbedEventsRoutine");
        }
        // Hopefully this will always go last after the other methods have been invoked...lets see.
        yield return new WaitForEndOfFrame();
        interactableFacade.FirstGrabbed = null;
    }

    public void toggleDistanceGrabOutline(InteractableFacade interactableFacadeComp)
    {
        bool isComponentFound = interactableFacadeComp.gameObject.TryGetComponent<Outline>(out Outline outlineScript);
        if (isComponentFound)
        {
            if (!interactableFacadeComp.IsTouched)
            {
                outlineScript.enabled = !outlineScript.enabled;
                if (outlineScript.enabled)
                {
                    interactableFacadeComp.DisableTouch();
                }
                else
                {
                    interactableFacadeComp.EnableTouch();
                }
            }
        }
    }
}

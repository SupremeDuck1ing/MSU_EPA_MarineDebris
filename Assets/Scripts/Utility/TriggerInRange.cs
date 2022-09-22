using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInRange : MonoBehaviour
{
    public UnityEvent eventsToTrigger;
    public Transform objectTransform;
    public int range = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(objectTransform != null);
        StartCoroutine(DistanceCheck());
    }

    IEnumerator DistanceCheck()
    {
        yield return new WaitUntil(InRange);
        eventsToTrigger?.Invoke();
    }

    bool InRange()
    {
        return Vector3.Distance(transform.position, objectTransform.position) < range;
    }
}

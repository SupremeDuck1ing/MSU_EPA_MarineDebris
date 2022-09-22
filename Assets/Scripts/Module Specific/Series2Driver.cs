using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Series2Driver : MonoBehaviour
{
    int grabbedFrames = 0;
    public UnityEvent nextEvents;
    public void Series2Task()
    {
        grabbedFrames += 1;
        if (grabbedFrames == 3)
        {
            nextEvents?.Invoke();
        }
    }
}

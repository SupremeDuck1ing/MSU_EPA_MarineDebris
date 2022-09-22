using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIDelegate))]
public class UITimer : MonoBehaviour
{
    public int timer = 0;
    public Text timerOutput;
    private UIDelegate uiDelegateScript; 
    public bool isGrabbed = false;

    void Awake()
    {
        Debug.Assert(timerOutput != null);
        uiDelegateScript = GetComponent<UIDelegate>();
        // this script will control the closing of UIDelegate
        uiDelegateScript.promptTimer = 0;
    }

    void Start()
    {
        if (timer > 0)
        {
            StartCoroutine(StartTimer());
        }
    }

    public void toggleGrab() 
    { 
        if(isGrabbed) 
        { 
            isGrabbed = false;
        } 
        else 
        { 
            isGrabbed = true;
        }
    }

    IEnumerator StartTimer()
    {
        string tempText = timerOutput.text;
        while (timer > 0 || isGrabbed)
        {
            yield return new WaitForSeconds(1);

            //timerOutput.text = tempText + timer;
            timer--;
        }
        uiDelegateScript.OnCloseAndContinue();
    }


}

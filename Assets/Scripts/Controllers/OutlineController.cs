//Controller Script for Interactable outlines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public bool doDefaultFlash = true;
    public bool doRejectOutline = true;
    private Outline outliner;
    private bool isDefaultFlashing;
    private bool isRejectOutlined;
    private Color orange = new Color32(255,117,0, 255);
    
    // Start is called before the first frame update
    void Start()
    {
        outliner = GetComponent<Outline>();
        StartCoroutine(DefaultFlash());
    }

    public void StartDefaultFlash()
    {
        StartCoroutine(DefaultFlash());
    }

    public void StartRejectOutline()
    {
        StartCoroutine(RejectOutline());
    }

    public void Success()
    {
        outliner.OutlineColor = Color.green;
        outliner.enabled = true;
    }


    //Turn the outline on and off in rapid succession to mimic flashing
    IEnumerator DefaultFlash()
    {
        isDefaultFlashing = true;
        outliner.OutlineColor = orange;
        while (doDefaultFlash)
        {
            outliner.enabled = !outliner.enabled;
            yield return new WaitForSeconds(1.5f);
        }
        outliner.enabled = false;
        doDefaultFlash = true;
        isDefaultFlashing = false;
    }

    //Changes outline to signal a rejection
    IEnumerator RejectOutline()
    {
        isRejectOutlined = true;
        outliner.OutlineColor = Color.red;
        outliner.enabled = true;
        while (doRejectOutline)
        {
            yield return null;
        }
        outliner.enabled = false;
        doRejectOutline = true;
        isRejectOutlined = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    
using UnityEngine.Events;

public class TrashBinLogic : MonoBehaviour
{
    TextMeshPro mText = null;
    [SerializeField]
    public GameObject nextEnable = null;
    public UnityEvent CallFunction = null;
    public AudioSource throwAwaySound;
    private SoundController soundController;
    void OnTriggerEnter(Collider col) 
    {
        switch (col.tag)
        {
            case "throwAway":
                Debug.Log("TRASH BIN SCRIPT OnTriggerEnter: " + col.tag);
                if (counterTest < 5)
                {
                    StartCoroutine(FlashGreen());
                    incrementCounter();
                    updateText();
                    col.gameObject.tag = "Untagged";
                    MeshRenderer[] meshRenderers = col.gameObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (var comp in meshRenderers)
                    {
                        comp.enabled = false;
                    }
                    
                    var isFound = col.gameObject.TryGetComponent<SoundController>(out soundController);
                    if (isFound)
                    {
                        soundController.ThrowmAway();
                    }
                }
                if (counterTest == 5)
                {
                    nextEnable.SetActive(true);
                    if (CallFunction != null)
                    {
                        CallFunction.Invoke();
                    }
                }
                break;
            default:
                Debug.Log("TRASH BIN SCRIPT OnTriggerEnter.");
                break;
        }
        
    }

    IEnumerator FlashGreen()
    {
        GetComponent<Outline>().enabled = true;
        yield return new WaitForSeconds(.5f);
        GetComponent<Outline>().enabled = false;
    }

    void OnEnable()
    {
        StartCoroutine(checkCounter());
    }

    [HideInInspector]
    public int counterTest = 0;
    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponentInChildren<TextMeshPro>();
        Debug.Assert(nextEnable != null);
    }

    IEnumerator checkCounter()
    {
        // does a check 5x per second.  Better optimization
        while (counterTest < 6)
        {
            yield return new WaitForSeconds(0.2f);
        }
        mText.color = new Color(0,255,0);
        if (CallFunction != null)
        {
            CallFunction.Invoke();
        }
    }

    void incrementCounter()
    {
        counterTest += 1;
    }

    void updateText()
    {
        mText.text = counterTest + " / 5";
    }
}

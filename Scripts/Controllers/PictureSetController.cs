//Imported Script that seems to control a question and answer system (Seems to have been depricated by Kieth's inspector-built system)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Malimbe.XmlDocumentationAttribute;

public class PictureSetController : MonoBehaviour
{
    private int numOfPictures;
    [HideInInspector]
    public bool correctAnswerGiven = false;

    private GameObject[] pictures = null;
    public GameObject nextCanvas = null;
    public GameObject nextEvent = null;
    public UnityEvent CallFunction = null;


    // Start is called before the first frame update
    void Start()
    {
        numOfPictures = transform.childCount;
        pictures = new GameObject[numOfPictures];
        for (int i = 0; i < numOfPictures; i++)        
        {
            pictures[i] = transform.GetChild(i).gameObject;
        }
    }

    public IEnumerator waitForAnswer()
    {
        while (!correctAnswerGiven)
        {
            yield return new WaitForSeconds(0.2f);
        }

        gameObject.SetActive(false);
        UIPicture.submitAllowed = true;
        if (nextEvent)
        {
            nextEvent.SetActive(true);
        }
        if (nextCanvas)
        {
            UIDriver.instance.ShowCanvas(nextCanvas, true);
        }
        if (CallFunction != null)
        {
            CallFunction.Invoke();
        }
    }

    void OnEnable()
    {
       UIDriver.instance.toggleItems(true);
       StartCoroutine(waitForAnswer());
    }

    public void ClosePictures()
    {
        foreach (var pictureItem in pictures)
        {
            pictureItem.SetActive(false);
        }
    }
}

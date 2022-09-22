using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malimbe.XmlDocumentationAttribute;

public class UIPicture : UIDelegate
{
    [field: Header("Question Attributes"), DocumentedByXml]
    [SerializeField]
    public bool correctAnswer = false;
    [SerializeField]
    public PictureSetController myPicController = null;
    public ChangeMatColor matChangeScript;

    public static bool submitAllowed = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Debug.Assert(myPicController != null);
        Debug.Assert(matChangeScript != null);
    }
    public IEnumerator PauseSubmission()
    {
        submitAllowed = false;
        yield return new WaitForSeconds(3);
        submitAllowed = true;
    }

    public IEnumerator SendControllerInfo()
    {
        submitAllowed = false;
        yield return new WaitForSeconds(1);
        myPicController.correctAnswerGiven = true;
    }

    public IEnumerator FlashRed()
    {
        // flash red 3x and block submit button until done
        submitAllowed = false;
        int numOfFlashes = 3;
        for (int i = 0; i < numOfFlashes; i++)          
        {
            matChangeScript.SetFrameColor(Color.red);
            yield return new WaitForSeconds(0.15f);
            matChangeScript.SetFrameColor(Color.white);
            yield return new WaitForSeconds(0.15f);
        }
        submitAllowed = true;
    }

    public override void OnCloseAndContinue()
    {
        if (correctAnswer && submitAllowed)
        {
            matChangeScript.SetFrameColor(Color.green);
            StartCoroutine(SendControllerInfo());
        }
        else if (submitAllowed)
        {
            StartCoroutine(PauseSubmission());
            StartCoroutine(FlashRed());
        }
    }
}

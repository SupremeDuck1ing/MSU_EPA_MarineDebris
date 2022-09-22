using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Malimbe.XmlDocumentationAttribute;

public class RadioQuestion : UIDelegate
{
    #region Sound
    [field: Header("Sound"), DocumentedByXml]
    [SerializeField]
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    private AudioSource thisAudio;
    #endregion

    #region Attributes
    [field: Header("Question Attributes"), DocumentedByXml]
    [SerializeField]
    public Toggle correctAnswer = null;
    #endregion
    private Toggle[] toggleList = null;
    [HideInInspector]
    public bool submitAllowed = true;


    public override void Awake()
    {
        base.Awake();
        Debug.Assert(correctAnswer != null);
        toggleList = GetComponentsInChildren<Toggle>();
        Debug.Assert(toggleList != null);
        thisAudio = GetComponent<AudioSource>();
    }

    public IEnumerator DelayExec()
    {
        if (correctSound != null)
        {
            thisAudio.PlayOneShot(correctSound);
        }
        yield return new WaitForSeconds(3);
        base.OnCloseAndContinue();
    }

    public IEnumerator FlashRed(Text selectedText)
    {
        if (incorrectSound != null)
        {
            thisAudio.PlayOneShot(incorrectSound);
        }
        // flash red 3x and block submit button until done
        submitAllowed = false;
        int numOfFlashes = 3;
        for (int i = 0; i < numOfFlashes; i++)          
        {
            selectedText.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            selectedText.color = Color.black;
            yield return new WaitForSeconds(0.15f);
        }
        submitAllowed = true;
    }

    public override void OnCloseAndContinue()
    {
        if (submitAllowed)
        {
            Text selectedText;
            foreach (var toggle in toggleList)
            {
                if (toggle.isOn)
                {
                    selectedText = toggle.gameObject.GetComponentInChildren<Text>();
                    if (toggle == correctAnswer)
                    {
                        selectedText.color = Color.green;
                        StartCoroutine(DelayExec());
                        return;
                    }
                    else
                    {
                        StartCoroutine(FlashRed(selectedText));
                        break;
                    }
                }
            }
        }
    }
    
}

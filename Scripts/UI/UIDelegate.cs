/*
    Author:  Keith Wilcox
    Purpose:  A queue-like system where Canvases are displayed one after the other in a sequence.  Adds listeners to buttons.
*/

using Malimbe.XmlDocumentationAttribute;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIDelegate : MonoBehaviour
{
    #region Main Settings
    /// <summary>
    /// Main settings for this UI
    /// </summary>
    [field: Header("Main Settings"), DocumentedByXml]
    /// <summary>
    /// If promptTimer equals 0, it doesn't use a timer and needs to be closed manually through CloseAndContinue() if desired
    /// </summary>
    public int promptTimer = 0;
    public float fadeSpeed = 0.5f;
    public bool fadeIn = true;
    private bool fadeOut = false;
    public AudioClip speechClip;
    protected AudioSource speechAudioSource;
    #endregion

    #region On Enable
    /// <summary>
    /// Events to occur after this game object is enabled.
    /// </summary>
    [field: Header("On Enable Event(s)"), DocumentedByXml]
    public UnityEvent EnableFunctions = null;
    #endregion

    #region Button Events
    /// <summary>
    /// The button used to continue to close the current Canvas and show the next if assigned
    /// </summary>
    [field: Header("UI Events"), DocumentedByXml]
    public GameObject closeAndContinue = null;
    /// <summary>
    /// Optional field for if you have a button to go back to the previous Canvas
    /// </summary>
    [SerializeField]
    public GameObject previousButton = null;
    #endregion

    #region Next Menu Reference
    /// <summary>
    /// Optional field if you have another canvas to show after this one. Mpte, all your UI Template Canvases should be disabled in the scene initially.
    /// </summary>
    [field: Header("Next Events"), DocumentedByXml]
    [SerializeField]
    public GameObject nextCanvas = null;
    [SerializeField]
    public GameObject nextEnable = null;
    #endregion

    public delegate void OnClick();
    [SerializeField]
    public UnityEvent CallFunction = null;
    public bool locoAndDistanceOnClose = true;
    public bool enablePointerOnNext = true;

    private List<Text> visibleTextComps = new List<Text>();
    private List<Image> visisbleImageComps = new List<Image>();

    


    public virtual void Awake()
    {
        speechAudioSource = GameObject.FindGameObjectWithTag("Speech").GetComponent<AudioSource>();
        if (fadeIn || fadeOut)
        {
            GetVisibleUIComponents();
        }
    }

    public virtual void Start()
    {
        Debug.Log("UIDelegate Start");
        if (closeAndContinue)
        {
            AddButtonDelegate(closeAndContinue, OnCloseAndContinue);
        }
        if (previousButton)
        {
            AddButtonDelegate(previousButton, OnCloseAndContinue);
        }
        UIDriver.isCanvasShowing = true;

        if (fadeIn)
        {
            foreach (Text text in visibleTextComps)
            {
                StartCoroutine(TextFadeIn(text));
            }
            foreach (Image image in visisbleImageComps)
            {
                StartCoroutine(ImageFadeIn(image));
            }
        }

        if (promptTimer != 0)
        {
            Invoke("OnCloseAndContinue", promptTimer);
        }
    }
    
    public virtual void AddButtonDelegate(GameObject continueButton,OnClick handler)
    {
        Button button = continueButton.GetComponent<Button>();
        button.onClick.AddListener(delegate { handler(); });
    }

    public virtual void OnCloseAndContinue()
    {
        UIDriver.instance.HideCanvas(this.gameObject, locoAndDistanceOnClose);

        if (nextCanvas)
        {
            UIDriver.instance.ShowCanvas(nextCanvas, enablePointerOnNext);
        }

        if (nextEnable)
        {
            nextEnable.SetActive(true);
        }

        if (CallFunction != null)
        {
            CallFunction.Invoke();
        }
    }

    public virtual void OnEnable()
    {
        if (EnableFunctions != null)
        {
            EnableFunctions.Invoke();
        }
        if (speechClip != null)
        {
            speechAudioSource.clip = speechClip;
            speechAudioSource.Play();
        }
    }

    public virtual void OnDisable()
    {
        if (speechClip != null)
        { 
            /*if (speechAudioSource.isPlaying) 
            {
                speechAudioSource.Stop();
            }*/
        
        }
    }

    public virtual IEnumerator TextFadeIn(Text text)
    {
        for (float i = 0; i <= 1; i += (Time.deltaTime * fadeSpeed))
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
            yield return null;
        }
    }

    public virtual IEnumerator ImageFadeIn(Image image)
    {
        for (float i = 0; i <= 1; i += (Time.deltaTime * fadeSpeed))
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
            yield return null;
        }
    }

    public virtual void GetVisibleUIComponents()
    {
        // Append to list all image and text components that are on this gameOjbect
        Text[] textComps = GetComponents<Text>();
        if (textComps != null)
        {
            for (int i = 0; i < textComps.Length; i++)
            {
                visibleTextComps.Add(textComps[i]);
            }
        }
        Image[] imageComps = GetComponents<Image>();
        if (imageComps != null)
        {
            for (int i = 0; i < imageComps.Length; i++)
            {
                visisbleImageComps.Add(imageComps[i]);
            }
        }

        // Append to list of all text and image components that are children, and the gameobject attached to is enabled
        textComps = GetComponentsInChildren<Text>();
        if (textComps != null)
        {
            for (int i = 0; i < textComps.Length; i++)
            {
                if (textComps[i].gameObject.activeSelf)
                {
                    // Set alpha to 0.
                    textComps[i].color = new Color(textComps[i].color.r, textComps[i].color.g, textComps[i].color.b, 0);
                    visibleTextComps.Add(textComps[i]);
                }
            }
        }
        
        imageComps = GetComponentsInChildren<Image>();
        if (imageComps != null)
        {
            for (int i = 0; i < imageComps.Length; i++)
            {
                if (imageComps[i].gameObject.activeSelf)
                {
                    imageComps[i].color = new Color(imageComps[i].color.r, imageComps[i].color.g, imageComps[i].color.b, 0);
                    visisbleImageComps.Add(imageComps[i]); 
                }
            }   
        }
    }
}

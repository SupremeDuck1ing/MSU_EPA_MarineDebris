using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(VideoPlayer))]
public class VideoEvent : MonoBehaviour
{
    private VideoPlayer videoPlayerComp = null;
    private RawImage rawImageComp = null;
    private bool hasRawImage = false;
    [SerializeField]
    public GameObject nextCanvas = null;
    public UnityEvent nextEvents;

    void Start()
    {
        videoPlayerComp = GetComponent<VideoPlayer>();
        rawImageComp = GetComponent<RawImage>();
         // when video ends, startNextEvent is called as an Event Handler
        videoPlayerComp.loopPointReached += startNextEvent;
        StartCoroutine(FadeIn());
    }

    void startNextEvent(UnityEngine.Video.VideoPlayer videoPlayer)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        videoPlayerComp.Play();
        videoPlayerComp.Pause();
        for (float i = 0; i <= 1; i += (Time.deltaTime * 0.5f))
        {
            // set color with i as alpha
            rawImageComp.color = new Color(1, 1, 1, i);
            yield return null;
        }

        videoPlayerComp.Play();
    }

    IEnumerator FadeOut()
    {
        // Fade out
        for (float i = 1; i >= 0; i -= (Time.deltaTime * 0.5f))
        {
            // set color with i as alpha
            rawImageComp.color = new Color(1, 1, 1, i);
            yield return null;
        }

        if (nextCanvas != null)
        {
            UIDriver.instance.ShowCanvas(nextCanvas, true);
        }

        if (nextEvents != null)
        {
            nextEvents.Invoke();
        }

        gameObject.SetActive(false);
    }
}

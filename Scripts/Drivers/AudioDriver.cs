
using Malimbe.XmlDocumentationAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDriver : MonoBehaviour
{

    #region General Settings
    [field: Header("General Settings"), DocumentedByXml]
    [SerializeField]
    private List<GameObject> toEnable = null;
    #endregion

    

    public void FadeAudio(AudioSource sound)
    {
        StartCoroutine(FadeAudioCoroutine(sound));
    }

    IEnumerator FadeAudioCoroutine(AudioSource sound,float fadeTime = 5)
    {
        float startVolume = sound.volume;
        float adjustedVolume = startVolume;

        while (adjustedVolume > 0)
        {
            adjustedVolume -= startVolume * Time.deltaTime / fadeTime;
            sound.volume = adjustedVolume;
            yield return null;
        }
        sound.Stop();
    }
}

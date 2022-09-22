using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{   
    
    public Sound[] sounds;
    [HideInInspector]
    public bool isFading;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.spatialBlend = 0.5f;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }


    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }


    IEnumerator DelayedSwitch(string stopName, string startName)
    {
        StartCoroutine(StartFade(stopName,1f));
        while (isFading)
        {
            yield return null;
        }
        Stop(stopName);
        yield return new WaitForSeconds(0.05f);
        Play(startName);
        Sound s = Array.Find(sounds, sound => sound.name == startName);
        StartCoroutine(StartFade(startName, 1f, s.volume));
    }

    public IEnumerator StartFade(string name, float duration, float targetVolume = 0)
    {
        isFading = true;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (targetVolume != 0)
        {
            targetVolume = s.volume;
        }
        float currentTime = 0;
        float start = s.source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        s.source.volume = targetVolume;
        isFading = false;
        yield break;
    }
}

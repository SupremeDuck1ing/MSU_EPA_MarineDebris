using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip throwAwayClip;
    public AudioClip[] genericThrowAways;
    private AudioSource audioComp;

    void Start()
    {
        audioComp = GetComponent<AudioSource>();
    }

    public void ThrowmAway()
    {
        if (throwAwayClip != null)
        {
            audioComp.PlayOneShot(throwAwayClip);
        }
        else if (genericThrowAways.Length > 0)
        {
            audioComp.PlayOneShot(GetRandomClip());
        }
    }

    private AudioClip GetRandomClip()
    {
        return genericThrowAways[UnityEngine.Random.Range(0, genericThrowAways.Length)];
    }
}

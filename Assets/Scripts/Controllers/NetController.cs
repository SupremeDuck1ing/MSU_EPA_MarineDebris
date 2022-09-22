//Controller Script for Interactable Net

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetController : MonoBehaviour
{
    protected MeshCollider meshColComp;
    protected AudioSource audioSource;
    public AudioClip[] audioClips;

    void Awake()
    {
        meshColComp= GetComponent<MeshCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOrientationState();
    }

    protected virtual void UpdateOrientationState()
    {
        // check if there is debri in the net
        DebriController debriControllerComp = gameObject.transform.parent.gameObject.GetComponentInChildren<DebriController>();
        if (debriControllerComp)
        {
            //.parent.parent.parent for gameobject that is actually being rotated (has interactableFacade)
            GameObject interactableObject = gameObject.transform.parent.parent.parent.gameObject;
            // if parent global x rotation between , 75-110, then update state to flipped, which means let the bottle go from the net
            if (interactableObject.transform.rotation.eulerAngles.x > 75 && interactableObject.transform.eulerAngles.x < 110)
            {
                audioSource.PlayOneShot(GetRandomClip());
                debriControllerComp.DetachFromNet();
            }
        }
    }

    public void PlayNetSound()
    {
        audioSource.PlayOneShot(GetRandomClip());
    }

    protected AudioClip GetRandomClip()
    {
        return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    }
}

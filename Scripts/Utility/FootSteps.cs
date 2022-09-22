using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
    [SerializeField]
    public TerrainIndex footstepType;
    [SerializeField]
    private AudioClip[] woodClips;
    [SerializeField]
    private AudioClip[] sandClips;
    private AudioSource audioSource;

    public enum TerrainIndex
    {
        WOOD,
        SAND
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        switch(footstepType)
        {
            case 0:
                return woodClips[UnityEngine.Random.Range(0, woodClips.Length)];
            default:
                return sandClips[UnityEngine.Random.Range(0, sandClips.Length)];
        }
    }
}
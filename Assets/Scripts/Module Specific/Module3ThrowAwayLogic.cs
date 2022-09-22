using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAProgressBar;

public class Module3ThrowAwayLogic : MonoBehaviour
{
    public Transform handle;
    public float lerpDuration;
    protected float valueToLerp;
    protected Rigidbody debriRigidComp;
    public GameObject progressBar;
    private LinearProgressBar linearProgressBarScript;
    public bool isContainerFull;
    public int itemCount = 0;
    public AudioClip successIncrement;
    public AudioClip successComplete;
    protected AudioClip swooshClip;
    protected AudioSource audioSource;
    protected AudioSource progressSource; 
    private GameState gameStateScript;
    public Mod3Controller Mod3; 

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        Debug.Assert(progressBar != null);
        linearProgressBarScript = progressBar.GetComponent<LinearProgressBar>();
        progressSource = gameObject.AddComponent<AudioSource>();
        progressSource.spatialBlend = 0.5f;
        swooshClip = audioSource.clip;  
        gameStateScript = GameObject.Find("GameState").GetComponent<GameState>();
        Mod3 = GameObject.Find("Module 3").GetComponent<Mod3Controller>(); 
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col) 
    { 
        switch (col.tag)
        {
            case "plastic":
                if (gameObject.tag == "plasticBin")
                {
                    ThrowAwayItem(col);
                }
                else
                {
                    RejectItem(col);
                }
                break;
            case "fastfood":
                if (gameObject.tag == "landfillBin")
                {
                    ThrowAwayItem(col);
                }
                else
                {
                    RejectItem(col);
                }
                break;
            case "metal":
                if (gameObject.tag == "metalBin")
                {
                    ThrowAwayItem(col);
                }
                else
                {
                    RejectItem(col);
                }
                break;
            case "rubber":
                if (gameObject.tag == "rubberBin")
                {
                    ThrowAwayItem(col);
                }
                else
                {
                    RejectItem(col);
                }
                break;
            default:
                break;
        }
        
    }

    protected virtual void ThrowAwayItem(Collider col)
    {
        itemCount += 1;
        col.enabled = false;
        linearProgressBarScript.FillAmount += .33f;
        col.gameObject.GetComponent<OutlineController>().Success();
        StartCoroutine(Lerp(col.gameObject.transform));
    }

    void RejectItem(Collider col)
    {
        col.gameObject.GetComponent<OutlineController>().StartRejectOutline();
        debriRigidComp = col.gameObject.GetComponent<Rigidbody>();
        debriRigidComp.AddForce(transform.up * 60000f);
    }

    public void ApplyForce()
    {
        debriRigidComp.AddForce(new Vector3(Random.Range(-0.2f,0.2f), 1, Random.Range(-0.2f, 0.2f)) * 60000f);
    }

    protected virtual IEnumerator Lerp(Transform objectsTransform)
    {
        float timeElapsed = 0;
        Vector3 startPos = objectsTransform.position;
        Vector3 endPos = handle.position;
        bool soundPlayed = false;
        while (timeElapsed < lerpDuration)
        {
            if (timeElapsed > (lerpDuration * 0.88f) && !soundPlayed)
            {
                soundPlayed = true;
                audioSource.PlayOneShot(swooshClip);
            }
            objectsTransform.position = Vector3.Lerp(startPos, endPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        if (itemCount < 3)
        {
            progressSource.clip = successIncrement;
            audioSource.PlayOneShot(successIncrement);
            
        }
        else
        {
            gameStateScript.updateStateAndMenu();
            progressSource.clip = successComplete;
            audioSource.PlayOneShot(successComplete);
            Mod3.binCount += 1;
        }

        objectsTransform.position = endPos;
        // prevents StopRolling coroutine from being performed
        objectsTransform.gameObject.GetComponent<PhysicsController>().isBeingDestroyed = true;
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(12,13,false);
        string debriTag = objectsTransform.gameObject.tag;
        Destroy(objectsTransform.gameObject);
        yield return new WaitForSeconds(0.5f);
        CheckToSpawnItem(debriTag);
    }

    public void CheckToSpawnItem(string tag)
    {
        var debug = GameObject.FindGameObjectWithTag(tag);
        if (itemCount < 3 && !GameObject.FindGameObjectWithTag(tag))
        {
            DebriSpawner.instance.GenerateSpawnLocation(tag);    
        }
    }
}

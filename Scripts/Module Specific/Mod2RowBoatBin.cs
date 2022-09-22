using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mod2RowBoatBin : Module3ThrowAwayLogic
{
    public int itemNewCount = 0;
    private GameObject vRTKPlayer; 
    private GameState gameState; 
    public GameObject Rowboat; 

    public Transform TurtleDestination; 
    public Transform ManateeDestination;
    Vector3 boatStartPos;
    Vector3 boatStartAngles;
    Vector3 boatEndPos; 
    Vector3 boatEndAngles; 
    TriggerEvents triggerEvents;
    public bool DestinationFlag = false; 
    public bool isGrabbed = false;


    protected override void Start()
    {
        vRTKPlayer = GameObject.FindGameObjectWithTag("VRTKPlayer"); 
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        progressSource = gameObject.AddComponent<AudioSource>();
        progressSource.spatialBlend = 0.5f;
        swooshClip = audioSource.clip; 
        triggerEvents = GetComponent<TriggerEvents>();
    }

    protected override void ThrowAwayItem(Collider col)
    {
        col.enabled = false;
        col.gameObject.GetComponent<OutlineController>().Success();
        StartCoroutine(Lerp(col.gameObject.transform));
    }

    void MoveRowBoat() 
        { 
            StartCoroutine(RowBoatLerp());
        } 

    IEnumerator RowBoatLerp() 
    { 
        float boatTimeElapsed = 0;
        if (itemNewCount == 3) 
        {  
            boatStartPos = Rowboat.GetComponent<Transform>().position;
            boatStartAngles = Rowboat.GetComponent<Transform>().eulerAngles;
            boatEndPos = TurtleDestination.position; 
            boatEndAngles = TurtleDestination.eulerAngles; 
        }   
        else 
        { 
            boatStartPos = Rowboat.GetComponent<Transform>().position;
            boatStartAngles = Rowboat.GetComponent<Transform>().eulerAngles;
            boatEndPos = ManateeDestination.position;
            boatEndAngles = ManateeDestination.eulerAngles; 
        }

        while(Rowboat.GetComponent<Transform>().position != boatEndPos) 
        {  
            Rowboat.GetComponent<Transform>().position = Vector3.Lerp(boatStartPos, boatEndPos, boatTimeElapsed / 16); 
            Rowboat.GetComponent<Transform>().eulerAngles = Vector3.Lerp(boatStartAngles, boatEndAngles, boatTimeElapsed / 16);
            boatTimeElapsed += Time.deltaTime; 
            yield return null;
        }
    }

    protected override IEnumerator Lerp(Transform objectsTransform)
    {
        itemNewCount += 1;
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

        progressSource.clip = successIncrement;
        audioSource.PlayOneShot(successIncrement);

        objectsTransform.position = endPos;
        // prevents StopRolling coroutine from being performed
        objectsTransform.gameObject.GetComponent<PhysicsController>().isBeingDestroyed = true;
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(12,13,false);
        string debriTag = objectsTransform.gameObject.tag;
        Destroy(objectsTransform.gameObject); 

        if (itemNewCount == 3) {
            Debug.Log("Manta Freed");
            gameState.updateStateAndMenu();
            MoveRowBoat();
        }  
        else if(itemNewCount == 6)
        { 
            Debug.Log("Turtle Freed");
            gameState.updateStateAndMenu();
            MoveRowBoat();
        }
        else if (itemNewCount == 9) 
        {   
            Debug.Log("Bin Full");
            gameState.updateStateAndMenu();
            triggerEvents.CallEvents();
            SceneTransition();
        } 
        else { 
            Debug.Log("Trash Thrown Away");
        }
    }
    public void SceneTransition()
    {
        if (SceneManager.GetActiveScene().name == "Module_1")
        {
            StartCoroutine(LoadYourAsyncScene("Module_2"));
        }
        else if (SceneManager.GetActiveScene().name == "Module_2")
        {
            vRTKPlayer.transform.parent = null;
            DontDestroyOnLoad(vRTKPlayer);
            StartCoroutine(LoadYourAsyncScene("Module_3"));
        }
    } 

    public void toggleGrab() 
    { 
        if(isGrabbed == true) 
        { 
            isGrabbed = false;
        } 
        else 
        { 
            isGrabbed = true;
        }
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        while (isGrabbed) 
        { 
            yield return null;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField]
    public bool interactablesIgnoreInteractor;
    [SerializeField]
    public Toggle[] taskToggles = null;
    [SerializeField]
    public Toggle[] moduleToggles = null;
    public bool[,] modulesTasks = new bool[3,3];
    private int currentState = 0;
    [SerializeField]
    public GameObject distanceGrabberObj = null;
    public static bool inTutorial = true;
    public static GameState instance;
    [HideInInspector]
    public GameObject grabbedItem;
    public GameObject tutorialObject;
    public List<GameObject> Mod1Cleanup;
    private GameObject vRTKPlayer;
    public bool delayEnableDistanceGrab = true;

    // Start is called before the first frame update
    
    void Awake()
    {
        instance = this;
        vRTKPlayer = GameObject.FindGameObjectWithTag("VRTKPlayer");
    }

    void Start()
    {
        if (interactablesIgnoreInteractor)
        {
            Physics.IgnoreLayerCollision(2,12, true);
        }
        if (delayEnableDistanceGrab)
        {
            StartCoroutine("DelayEnableGameObject", distanceGrabberObj);
        }
        Debug.Assert(taskToggles.Length == 10);
        Debug.Assert(moduleToggles.Length == 3);
        Debug.Assert(distanceGrabberObj != null);
        Debug.Assert(tutorialObject != null);
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

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator DelayEnableGameObject(GameObject myObj)
    {
        yield return new WaitForSeconds(1);
        myObj.SetActive(true);
    }

    public void updateStateAndMenu() 
    { 
        StartCoroutine(updateStateAndMenuRoutine());
    }
    public IEnumerator updateStateAndMenuRoutine()
    {
        ColorBlock cb = taskToggles[currentState].colors;
        ColorBlock modCb = taskToggles[currentState].colors;
        cb.disabledColor = new Color(0f,255f,0f,1f);
        taskToggles[currentState].colors = cb;
        int currModule = (currentState+1) / 3;
        switch (currentState)
        {
            case 2:
                modCb = taskToggles[currentState].colors;
                modCb.disabledColor = new Color(0f,255f,0f,1f);
                modCb.normalColor = new Color(0f,255f,0f,1f);;
                modCb.pressedColor = new Color(0f,255f,0f,1f);
                modCb.selectedColor = new Color(0f,255f,0f,1f);
                moduleToggles[currModule-1].colors = modCb;
                break;                
            case 5:
                modCb = taskToggles[currentState].colors;
                modCb.disabledColor = new Color(0f,255f,0f,1f);
                modCb.normalColor = new Color(0f,255f,0f,1f);;
                modCb.pressedColor = new Color(0f,255f,0f,1f);
                modCb.selectedColor = new Color(0f,255f,0f,1f);
                moduleToggles[currModule-1].colors = modCb;
                break;
            case 9:
                modCb = taskToggles[currentState].colors;
                modCb.disabledColor = new Color(0f,255f,0f,1f);
                modCb.normalColor = new Color(0f,255f,0f,1f);;
                modCb.pressedColor = new Color(0f,255f,0f,1f);
                modCb.selectedColor = new Color(0f,255f,0f,1f);
                moduleToggles[2].colors = modCb;
                break;
            default:
                break;
        }
        //modulesTasks[currModule,currentState] = true;
        Debug.Log(currentState);
        currentState++; 
        yield return null;
    }

    //Transitions to next scene, but updates player menu and other relevent stats to preserve program flow
    public void skipModule(){ 
        StartCoroutine(skipModuleRoutine());
    } 

    public IEnumerator skipModuleRoutine() { 

        if(SceneManager.GetActiveScene().name == "Module_1") {
            currentState = 0; 
            for (int cnt = 0; cnt < 3; cnt++) { 
                updateStateAndMenu();
            }
        }  
        else if(SceneManager.GetActiveScene().name == "Module_2") { 
            currentState = 3; 
            for (int cnt = 0; cnt < 3; cnt++) { 
                updateStateAndMenu();
            }
        } 
        else { 
            Debug.Log("Can't skip further.");
        }

        SceneTransition();
        return null;
    }

    public void updateStateVariables()
    {
        grabbedItem = GameObject.FindGameObjectWithTag("GrabbedItem");
    }

    public void CleanUpTutorial()
    {
        Destroy(tutorialObject);
    }

    public void CleanUpMod1()
    {
        // can't destroy immediately, since this function is being called by an object we are destroying
        StartCoroutine(CleanUpMod1Routine());
    }

    IEnumerator CleanUpMod1Routine()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject obj in Mod1Cleanup)
        {
            Destroy(obj);
        }
    }
}

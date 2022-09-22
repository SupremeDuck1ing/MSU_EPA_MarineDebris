using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Locomotors.Teleporter;
using UnityEngine.SceneManagement;

public class Mod1Controller : MonoBehaviour
{
    GameObject vrtkPlayer;
    public Transform mod1Teleport;
    TeleporterFacade teleporter; 
    UIDriver uIDriver;

    void Start()
    {
        teleporter = FindObjectOfType<TeleporterFacade>();
        vrtkPlayer = GameObject.FindGameObjectWithTag("VRTKPlayer");
        uIDriver = GameObject.FindGameObjectWithTag("UIDriver").GetComponent<UIDriver>();
        StartModule1();
    }
    // Start is called before the first frame update
    public void StartModule1()
    {
        teleporter.Teleport(mod1Teleport);  
        //uIDriver.EnableMod3Tools();  
        //uIDriver.DisableAllTools();
        uIDriver.EnableLaserPointer(); 
    }  
}

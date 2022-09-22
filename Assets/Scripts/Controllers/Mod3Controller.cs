//Controller Script for Module 3

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tilia.Locomotors.Teleporter;
using UnityEngine.SceneManagement; 


public class Mod3Controller : MonoBehaviour
{
    GameObject vrtkPlayer;
    public Transform mod3Teleport;
    public AudioSource endAudio;
    Transform endteleportPos; 
    Transform OVRrig;
    TeleporterFacade teleporter; 
    GameObject ArrowL; 
    GameObject ArrowR;  
    GameObject VRCamera;  
    TriggerEvents trig;
    public int binCount = 0;  
    public Component[] cameraTransforms;
    

    
    UIDriver uIDriver;
    void Start()
    {
        teleporter = FindObjectOfType<TeleporterFacade>(); 
        endteleportPos = GameObject.Find("Mod1VRTKPos").GetComponent<Transform>();
        vrtkPlayer = GameObject.FindGameObjectWithTag("VRTKPlayer");
        uIDriver = GameObject.FindGameObjectWithTag("UIDriver").GetComponent<UIDriver>(); 
        VRCamera = GameObject.Find("TrackingSpace");   
        OVRrig = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        cameraTransforms = VRCamera.GetComponentsInChildren<Transform>(); 
        trig = GetComponent<TriggerEvents>();
        StartModule3();
    } 

    //Checks for end state and updates for the ending
    void Update()
    { 
        if(binCount == 4)
        { 
            endTeleport(); 
            trig.CallEvents();
        }
    }
    // Initial tool and placement setup for mod 3
    public void StartModule3()
    {
        teleporter.Teleport(mod3Teleport);    
        uIDriver.DisableAllTools();
        uIDriver.EnableLaserPointer();
    }   

    public void endTeleport()
    { 
        StartCoroutine(endTeleportRoutine());
    } 

    //Moves camera to ending position, bit clunky due to nature of vr

    IEnumerator endTeleportRoutine() 
    { 
        
        vrtkPlayer.GetComponent<Transform>().position = new Vector3 (endteleportPos.position.x, endteleportPos.position.y, endteleportPos.position.z); 
        VRCamera.GetComponent<Transform>().position = new Vector3 (0, 0, 0);
        foreach (Transform pos in cameraTransforms)
        { 
            pos.position = new Vector3 (0, 0, 0);
        } 
        uIDriver.EnableLaserPointer();
        yield return null;
    }

    //Originally supposed to restart application, changed to Quit() due to technical issues
    public void Restart() 
    { 
        Debug.Log("Application End");
        Application.Quit();  
        
    }
    
    //modified initial teleport function

    public void rotateTeleport() 
    { 
        uIDriver.EnableMod3Tools();
        uIDriver.toggleItems(false); 
        StartCoroutine(rotateTeleportRoutine());
    } 

    //teleport arrows must be rotated in module 3 to ensure proper traversal
    IEnumerator rotateTeleportRoutine() 
    { 
        Transform[] trans = GameObject.Find("Elements.CylinderL").GetComponentsInChildren<Transform>(true); 
        foreach (Transform t in trans) 
        { 
            if (t.gameObject.name == "ValidContainer") 
            { 
                Transform[] ArrowLTrans = t.gameObject.GetComponentsInChildren<Transform>(true); 
                foreach (Transform j in ArrowLTrans) 
                { 
                    if (j.gameObject.name == "ArrowValid") 
                    { 
                        ArrowL = j.gameObject;
                        ArrowL.transform.Rotate(0, 90, 0); 
                        ArrowL.transform.localPosition = new Vector3(-.16f, .1f, -.002f);
                    }
                }
            }
        }

        Transform[] transR = GameObject.Find("Elements.CylinderR").GetComponentsInChildren<Transform>(true); 
        foreach (Transform i in transR) 
        { 
            if (i.gameObject.name == "ValidContainer") 
            { 
                Transform[] ArrowRTrans = i.gameObject.GetComponentsInChildren<Transform>(true); 
                foreach (Transform j in ArrowRTrans) 
                { 
                    if (j.gameObject.name == "ArrowValid") 
                    { 
                        ArrowR = j.gameObject;
                        ArrowR.transform.Rotate(0, 90, 0);
                        ArrowR.transform.localPosition = new Vector3(-.16f, .1f, -.002f);
                    }
                }
            }
        }
        yield return null;
    }

}
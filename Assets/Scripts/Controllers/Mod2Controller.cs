//Controller Script for Module 2, basically responsible for kickstarting the module

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Locomotors.Teleporter;

public class Mod2Controller : MonoBehaviour
{
    public Transform boatTeleport;
    TeleporterFacade teleporter;
    GameObject vrtkPlayer;
    GameObject userInterface;
    GameObject mod2Boat;
    UIDriver uIDriver;

    //collects references to the player, ui, and big boat
    void Start()
    {
        teleporter = FindObjectOfType<TeleporterFacade>();
        vrtkPlayer = GameObject.FindGameObjectWithTag("VRTKPlayer");
        mod2Boat = GameObject.FindGameObjectWithTag("Boat");
        uIDriver = GameObject.FindGameObjectWithTag("UIDriver").GetComponent<UIDriver>();
        userInterface = GameObject.Find("User Interface");
        StartModule2();
    }
    
    //Transfers the player to the big boat, parents the ui and player under the boat, and enables the boat controller script to start the lerp function
    public void StartModule2()
    {
        teleporter.Teleport(boatTeleport);
        uIDriver.DisableTeleporting();
        vrtkPlayer.transform.parent = mod2Boat.transform;
        userInterface.transform.parent = vrtkPlayer.transform;
        mod2Boat.GetComponent<BoatController>().enabled = true;
    }
}

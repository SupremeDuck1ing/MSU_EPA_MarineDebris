//Controller Script for the RowBoat

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Locomotors.Teleporter;

public class RowBoatController : BoatController
{
    public bool moveForward;
    public Transform rowBoatDestination;
    public Transform rowBoatSeat;
    public GameObject rowBoatRotator;
    TeleporterFacade teleporter;
    GameObject vrtkPlayer;
    UIDriver uIDriver;
    private int activatedCalls;
    private int deactivatedCalls;
    private bool bStopTurn;
    public float turnSpeed = 0.5f;

    public float rotateSpeed = 30;
    private bool isTurning;

    // Start is called before the first frame update
    void Start()
    {
        teleporter = FindObjectOfType<TeleporterFacade>();
        uIDriver = GameObject.FindGameObjectWithTag("UIDriver").GetComponent<UIDriver>();
        vrtkPlayer = GameObject.FindGameObjectWithTag("VRTKPlayer");
        Debug.Assert(rowBoatDestination != null);
        Debug.Assert(rowBoatSeat != null);
        Debug.Assert(rowBoatRotator != null);
    }

    
    //Starts Rowboat movement and allows for distance grabbing when the big boat and rowboat colliders enter each other
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "node-0")
        {
            GetComponent<Collider>().enabled = false;
            GameObject debug = col.gameObject;
            col.gameObject.transform.parent.gameObject.GetComponent<BoatController>().enabled = false;
            uIDriver.EnableDistanceGrab();
            StartCoroutine(StartRowboatCoroutine());
        }
    }

    IEnumerator StartRowboatCoroutine()
    {
        yield return new WaitForSeconds(1f);
        teleporter.Teleport(rowBoatSeat);
        vrtkPlayer.transform.parent = transform; 

        yield return new WaitForSeconds(1.5f);
        StartMoveRowBoat();
    }

    void StartMoveRowBoat() 
        { 
            StartCoroutine(StartRowBoatLerp());
        } 

    IEnumerator StartRowBoatLerp() 
    { 
        float boatTimeElapsed = 0;
        Vector3 startPos = GetComponent<Transform>().position;
        Vector3 endPos = rowBoatDestination.position; 
        Vector3 startAngles = GetComponent<Transform>().eulerAngles; 
        Vector3 endAngles = rowBoatDestination.eulerAngles;  

        while(GetComponent<Transform>().position != endPos) 
        {  
            GetComponent<Transform>().position = Vector3.Lerp(startPos, endPos, boatTimeElapsed / 16); 
            GetComponent<Transform>().eulerAngles = Vector3.Lerp(startAngles, endAngles, boatTimeElapsed / 16);
            boatTimeElapsed += Time.deltaTime; 
            yield return null;
        }
    }

    public void HorizontalActivated(float fHorizontal)
    {
        Debug.Log("Horizontal Activated ( " + activatedCalls + " ): fHorizontal " + fHorizontal);
    }

    public void TurnRowBoat(float fHorizontal)
    {
        if (!isTurning)
        {
            StartCoroutine(TurnRowboatCoroutine(fHorizontal));
        }
    }

    IEnumerator TurnRowboatCoroutine(float fHorizontal)
    {
        isTurning = true;
        bStopTurn = false;
        while (!bStopTurn)
        {
            float angle = rotateSpeed * Time.deltaTime;
            if (fHorizontal < 0)
            {
                // turn left
                transform.rotation *= Quaternion.AngleAxis(angle, -Vector3.up);
            }
            else
            {
                // turn right
                transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
            }
            
            yield return null;
        }
        isTurning = false;
    }

    public void StopTurn()
    {
        bStopTurn = true;
    }

    public void HorizontalDeactivated(float fHorizontal)
    {
        Debug.Log("Horizontal Deactivated ( " + deactivatedCalls + " ): fHorizontal " + fHorizontal);
    }
}
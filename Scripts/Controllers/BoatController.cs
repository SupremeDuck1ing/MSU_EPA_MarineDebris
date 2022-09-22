//Base Boat Controller Script with Movement Coroutine 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float speed;
    public Transform BigBoatDestination;

    // Start is called before the first frame update
    void Start()
    {
        MoveBigBoat();
    }

    
    //Lerp Fucntion moves the Big Boat between start and end points rather than incrementing along an axis
    void MoveBigBoat() 
        { 
            StartCoroutine(BigBoatLerp());
        } 

    IEnumerator BigBoatLerp() 
    { 
        float boatTimeElapsed = 0;
        Vector3 startPos = GetComponent<Transform>().position;
        Vector3 endPos = BigBoatDestination.position; 

        while(GetComponent<Transform>().position != endPos) 
        {  
            GetComponent<Transform>().position = Vector3.Lerp(startPos, endPos, boatTimeElapsed / 40); //Changing the divisor to change the boat's speed
            boatTimeElapsed += Time.deltaTime; 
            yield return null;
        }
    }
}

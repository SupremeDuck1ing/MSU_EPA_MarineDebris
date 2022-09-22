//Controller Script to handle debris &/or the net falling into inaccessible places (i.e underwater or under the beach)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallthroughController : MonoBehaviour
{
    public Transform resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(resetPosition != null);
    }

    //On collision with the water collider (which is slightly below the surface), the object w/ this script attached returns to the position specified in the inspector
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 4)
        {
            StartCoroutine(ReturnNet());
        } 

    }

    IEnumerator ReturnNet() 
    { 
        yield return new WaitForSeconds(2.5f);
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = resetPosition.position;
        transform.rotation = resetPosition.rotation;
    }
}

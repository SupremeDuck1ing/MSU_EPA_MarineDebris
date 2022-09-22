using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriSpawner : MonoBehaviour
{
    public GameObject[] plastics;

    public GameObject[] metals;
    public GameObject[] rubbers;
    public GameObject[] fastfoods;
    public Transform areaBound1;
    public Transform areaBound2;
    public static DebriSpawner instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Assert(areaBound1 != null);
        Debug.Assert(areaBound2 != null);
    }


    public void GenerateSpawnLocation(string tag)
    {
        bool validSpawn = false;
        int randomXpos;
        int randomZpos;
        RaycastHit hit;
        while (!validSpawn)
        {
            randomXpos = Random.Range((int)areaBound1.position.x, (int)areaBound2.position.x);
            randomZpos = Random.Range((int)areaBound1.position.z, (int)areaBound2.position.z);
            Vector3 generatedSpawn = new Vector3(randomXpos,40,randomZpos);
            bool hitCollider = Physics.Raycast(generatedSpawn, transform.TransformDirection(Vector3.down), out hit);
            // did raycast hit a collider, and is that collider on the teleportable layer?
            if (hitCollider && hit.collider.gameObject.layer == 9)
            {
                Debug.DrawRay(generatedSpawn, transform.TransformDirection(Vector3.down) * hit.distance, Color.green, 10, false);
                hit.point += new Vector3(0,0,2);
                switch (tag)
                {
                    case "plastic":
                    Debug.Log("spawned " + tag);
                        Instantiate(plastics[UnityEngine.Random.Range(0, plastics.Length)], hit.point, Quaternion.identity);
                        break;
                    case "metal":
                        Debug.Log("spawned " + tag);
                        Instantiate(metals[UnityEngine.Random.Range(0, metals.Length)], hit.point, Quaternion.identity);
                        break;
                    case "rubber":
                    Debug.Log("spawned " + tag);
                        Instantiate(rubbers[UnityEngine.Random.Range(0, rubbers.Length)], hit.point, Quaternion.identity);
                        break;
                    case "fastfood":
                    Debug.Log("spawned " + tag);
                        Instantiate(fastfoods[UnityEngine.Random.Range(0, fastfoods.Length)], hit.point, Quaternion.identity);
                        break;
                    default:
                        break;
                }
                
                validSpawn = true;
            }
        }
        
    }

    public enum DebriTypes
    {
        PLASTIC,
        RUBBER,
        METAL,
        FASTFOOD

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    public List<Transform> destinationTransforms;
    public List<GameObject> objectsToMove;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(destinationTransforms.Count == objectsToMove.Count);
    }

    public void Move()
    {
        for (int i = 0; i < destinationTransforms.Count; i++)
        {
            objectsToMove[i].transform.position = destinationTransforms[i].position;
        }
    }
}

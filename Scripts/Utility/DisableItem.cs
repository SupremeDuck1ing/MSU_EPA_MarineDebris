using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableItem : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider col;
    public GameObject destroyObject;

    public void DisableObjectDifferent()
    {
        StartCoroutine(DelayedDisable());
        
    }
    void Start()
    {
        col = GetComponent<Collider>();
    }

    IEnumerator DelayedDisable()
    {
        yield return new WaitForSeconds(.25f);
        col.gameObject.SetActive(false);
    }

    public void DestroyObjectDifferent()
    {
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        destroyObject.transform.position = new Vector3(0,0,0);
        yield return new WaitForSeconds(1);
        if (destroyObject != null)
        {
             Destroy(destroyObject);   
        }
    }
}

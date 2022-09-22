using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEnable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enableItems;
    public int delay;

    // Update is called once per frame
    public void DelayEnableItems()
    {
        StartCoroutine(DelayEnableItemsRoutine());
    }

    IEnumerator DelayEnableItemsRoutine()
    {
        yield return new WaitForSeconds(delay);
        if (enableItems != null)
        {
            foreach (var item in enableItems)
            {
                item.SetActive(true);
            }
        }
    }
}

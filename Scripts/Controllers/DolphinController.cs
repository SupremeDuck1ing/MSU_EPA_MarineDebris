//*Controller Script for dolphin animations at beginning of mod 2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinController : MonoBehaviour
{
    public Transform underWaterTrans;
    public Transform fishFood;
    public Transform dolphinStart;
    public Transform fishFoodTrans;
    public Transform dolphinEndUnderWater;
    private Animator animator;
    private AudioManager audioManager; 
    public GameObject LookCanvas;
    public GameObject DeathCanvas;
    bool isLerping;
    public float riseAboveWaterDuration = 1f;
    public float moveToFoodDuration = 4f;
    public float endUnderWaterDuration = 3f;
    UIDriver uIDriver;

    //Collect Dolphin animator and audiomanager references
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }

    //Function triggers when the big boat collides w/ custom dolphin collider to trigger animation
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Dolphin OnTriggerEnter: " + col.gameObject.name);
        GetComponent<Collider>().enabled = false;
        uIDriver.DisableTeleporting();
        StartFishEvent1();
    }

    public void StartFishEvent1()
    {
        StartCoroutine(ConsumeDebri());
    }

    //Current Dolphin Animation Function 
    IEnumerator ConsumeDebri()
    {
        // rise above water
        StartCoroutine(Lerp(transform, underWaterTrans, dolphinStart, riseAboveWaterDuration)); 
        LookCanvas.SetActive(true);
        audioManager.Play("DolphinCheer");
        while (isLerping)
        {
            yield return null;
        }
        // move to debri
        StartCoroutine(Lerp(transform, dolphinStart, fishFoodTrans, moveToFoodDuration));
        
        while (Vector3.Distance(transform.position, fishFood.position) > 6f)
        {
            yield return null;
        }

        // eat debri
        animator.SetTrigger("eatTrig");

        yield return new WaitForSeconds(1f);
        Destroy(fishFood.gameObject);

        // swim again
        animator.SetTrigger("swimTrig");
        while (Vector3.Distance(transform.position, fishFoodTrans.position) > 1)
        {
            yield return null;
        }
        // death
        animator.SetTrigger("deathTrig");   
        DeathCanvas.SetActive(true);

    }

    public void StartDolphinJump()
    {
        StartCoroutine(DolphinJump());
    }

    //? Not entirely sure if this is used currently, may be depricated animation function
    IEnumerator DolphinJump()
    {
        StartCoroutine(Lerp(transform, underWaterTrans, dolphinStart, riseAboveWaterDuration));
        while (isLerping)
        {
            yield return null;
        }
        StartCoroutine(Lerp(transform, dolphinStart, fishFoodTrans, moveToFoodDuration));
        animator.SetTrigger("jumpTrig");
        audioManager.Play("DolphinCheer");
        
        yield return new WaitForSeconds(1.25f);

        animator.SetTrigger("swimTrig");
        while (isLerping)
        {
            yield return null;
        }
        
        StartCoroutine(Lerp(transform, fishFoodTrans, dolphinEndUnderWater, endUnderWaterDuration));
    }

    //Basic Lerp function to move dolphin from 1 position to another
    IEnumerator Lerp(Transform objectToLerp, Transform start, Transform stop, float duration)
    {
        isLerping = true;
        float time = 0;
        
        while (time < duration)
        {
            objectToLerp.position = Vector3.Lerp(start.position, stop.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objectToLerp.position = stop.position;
        isLerping = false;
    }
}

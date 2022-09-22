using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class AnimDriver : MonoBehaviour
{
    public enum EntityType
    {
        PLAYER,
        INTERACTABLE,
        OTHER
    }
    
    // Start is called before the first frame update
    public Transform player;
    public Transform goal;
    public AudioSource ambience;
    public Animator anim;
    private NavMeshAgent navMeshAgent;
    public float fadeTime = 0;
    public GameObject itemToGrab;
    private Transform itemTransform;
    private bool itemGrabbed;
    public GameObject hand;
    public static AnimDriver instance;
    public Transform frameTransform;
    private Vector3 previousPosition;

    void Start()
    {
        instance = this;
        
        Debug.Log("Started AnimDriver.");
        
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("HeadsetAlias").GetComponent<Transform>();

        StartCoroutine(InRangeEvents());
    }

    IEnumerator InRangeEvents()
    {
        yield return new WaitUntil(PlayerInRange);
        StartCoroutine(FadeOutAudio());
    }

    IEnumerator FadeOutAudio()
    {
        float startVolume = ambience.volume;
        float adjustedVolume = startVolume;

        while (adjustedVolume > 0)
        {
            adjustedVolume -= startVolume * Time.deltaTime / fadeTime;
            ambience.volume = adjustedVolume;
            yield return null;
        }
        ambience.Stop();
    }

    public void GrabAndGiveObjectSequence()
    {
        StartCoroutine(GrabAndGiveItem(true));
    }
    IEnumerator GrabAndGiveItem(bool returnToPreviousPos)
    {
        if (returnToPreviousPos)
        {
            previousPosition = transform.position;
        }

        StartCoroutine(MoveToTarget());

        // animation state behavior script will
        // focus NPC on item, call GrabObject(), and then focus on the player.
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(IsIdle);
        
        anim.SetBool("isGrabbing", true);
        anim.SetTrigger("isGrabbingTrig");
        
        yield return new WaitUntil(ItemGiven);
        yield return new WaitUntil(IsIdle);
        
        if (returnToPreviousPos)
        {
            StartCoroutine(MoveToTarget(previousPosition));
        }
        
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(IsIdle);
        
        FocusOnObject(EntityType.PLAYER);
    }

    public void ClapAndRewardSequence()
    {
        StartCoroutine(ClapAndReward());
    }

    IEnumerator ClapAndReward()
    {
        FocusOnObject(EntityType.PLAYER);
        anim.SetBool("isClapping", true);
        yield return new WaitForSeconds(4);
        anim.SetBool("isClapping", false);
    }

    IEnumerator MoveToTarget(Vector3 destination = new Vector3())
    {
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        anim.SetBool("isWalking", true);
        if (destination == new Vector3())
        {
             agent.destination = goal.position;
        }
        else
        {
            goal.position = destination;
            agent.destination = goal.position;
        }
        yield return new WaitUntil(IsAtGoal);
        anim.SetBool("isWalking", false);
    }

    bool ItemGiven()
    {
        return anim.GetBehaviour<GiveBehavior>().itemGrabbed;
    }

    bool IsAtGoal()
    {
        var isAtGoal = (transform.position.x == goal.position.x && transform.position.z == transform.position.z);
        return isAtGoal;
    }

    bool IsIdle()
    {
        var isIdle = anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        return isIdle;
    }

    bool IsAnimating()
    {
        var isAnimating = (anim.GetCurrentAnimatorStateInfo(0).normalizedTime != 100);
        return isAnimating;
    }

    bool PlayerInRange()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        return (dist <= 30);
    }

    public void GrabObject()
    {
        // backup item's position before the grab.  We may put the item back later.
        itemTransform = itemToGrab.transform;
        itemToGrab.transform.parent = hand.transform;
        itemToGrab.transform.position = frameTransform.position;
        itemToGrab.transform.eulerAngles = frameTransform.eulerAngles;
    }

    public void FocusOnObject(EntityType grabType)
    {
        switch (grabType)
        {
            case EntityType.INTERACTABLE:
                transform.LookAt(itemToGrab.transform);
                break;
            case EntityType.PLAYER:
                transform.LookAt(player);
                break;
            default:
                break;
        }
    }

    public IEnumerator HandItem()
    {
        yield return new WaitForSeconds(1);
        anim.SetBool("isGiving", true);
        anim.SetTrigger("isGivingTrig");
    }
}

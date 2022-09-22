using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehavior : StateMachineBehaviour
{    
    bool bGrabbed = false;
    AnimDriver animDriverScript = null;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animDriverScript = animator.GetComponent<AnimDriver>();
        animDriverScript.FocusOnObject(AnimDriver.EntityType.INTERACTABLE);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.47 && !bGrabbed)
        {
            bGrabbed = true;
            animDriverScript.GrabObject();
        }    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset flag
        bGrabbed = false;
        animDriverScript.FocusOnObject(AnimDriver.EntityType.PLAYER);
        animDriverScript.StartCoroutine(animDriverScript.HandItem());
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

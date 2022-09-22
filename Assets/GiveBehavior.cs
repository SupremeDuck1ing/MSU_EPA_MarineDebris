using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;

public class GiveBehavior : StateMachineBehaviour
{
    public bool itemGrabbed = false;
    public bool handExtended = false;
    AnimDriver animDriverScript = null;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animDriverScript = animator.GetComponent<AnimDriver>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.65 && !handExtended)
        {
            // pause the animation when the hand is extended
            animator.speed = 0;
            handExtended = true;

            // enable the interactable facade so that it's grabbable
            animator.GetComponentInChildren<InteractableFacade>().enabled = true;
        }

        if (itemGrabbed)
        {
            itemGrabbed = false;
            // player grabbed the item, resume animation.
            animator.speed = 1;
            // transition to idle state by setting the parameters for the animator
            animator.SetBool("isGiving", false);
            animator.SetBool("isGrabbing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

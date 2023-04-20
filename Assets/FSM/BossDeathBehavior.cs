using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Hurtbox hb = animator.gameObject.GetComponent<Hurtbox>();
       AttackManager am = animator.gameObject.GetComponent<AttackManager>();
    //hb.InvokeFlash(0.2f, Color.magenta, false, false, 100, 0.2f);
        hb.invin = true; 
        am.StopPlay();
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Destroy();
       // Destroy(animator.gameObject);   
    }

 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2BossBehavior : StateMachineBehaviour
{
    public Transform player;
    public int currState;
    public float speed;
    public float lineOfSightDistance;
    public float minimumDistance;
    public GameObject bulletPrefab;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("Player").transform;

        currState = Random.Range(1,4);
        Debug.Log("Time for phase 2" + currState);

    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if curr is basic attack
       if(currState == 1)
       {
            if(Vector2.Distance(animator.gameObject.transform.position, player.position) > minimumDistance)
            {
                animator.gameObject.transform.position = Vector2.MoveTowards(animator.gameObject.transform.position, player.position, speed * Time.deltaTime);

            }
            else
            {
                animator.SetTrigger("BasicAttack");
            }
       }
       else if(currState == 2)
       {
           animator.SetTrigger("ShootAttack");
       }
       else if(currState == 3)
       {
            animator.SetTrigger("ChargeAttack");
            //animator.gameObject.transform.position = Vector2.MoveTowards(animator.gameObject.transform.position, animator.gameObject.transform.position)
       }
       
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
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

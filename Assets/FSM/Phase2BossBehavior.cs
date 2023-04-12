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
    public AttackManager am;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        speed = 5;
        am = animator.gameObject.GetComponent<AttackManager>();
        player = GameObject.Find("Player").transform;

        currState = Random.Range(1,4);
        Debug.Log("Time for phase 2" + currState);
        if(Vector2.Distance(animator.gameObject.transform.position, player.position) > 6)
        {
            speed = 12;
            //currState = 1;
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.gameObject.transform.position.x > player.position.x)
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(animator.gameObject.transform.localScale.x);
            animator.gameObject.transform.localScale = new Vector3(-newX, animator.gameObject.transform.localScale.y, animator.gameObject.transform.localScale.z);
            //Debug.Log("Flipping should happen");
        }
        else
        {
            float newX = Mathf.Abs(animator.gameObject.transform.localScale.x);
            animator.gameObject.transform.localScale = new Vector3(newX, animator.gameObject.transform.localScale.y, animator.gameObject.transform.localScale.z);
            //sr.flipX = false;
        }
        //currState = 1;
        //Debug.Log(currState);       
        
                 // if curr is basic attack

       if(currState == 1)
       {
            if(Vector2.Distance(animator.gameObject.transform.position, player.position) > minimumDistance)
            {
                animator.gameObject.transform.position = Vector2.MoveTowards(animator.gameObject.transform.position, player.position, speed * Time.deltaTime);

            }
            else
            {
                Debug.Log("invoking");
                am.InvokeAttack("Basic Attack");
                //animator.SetTrigger("BasicAttack");
            }
       }
       else if(currState == 2)
       {
            am.InvokeAttack("Shoot at Player");
           //animator.SetTrigger("ShootAttack");
       }
       else if(currState == 3)
       {
             if(Vector2.Distance(animator.gameObject.transform.position, player.position) > minimumDistance)
            {
                animator.gameObject.transform.position = Vector2.MoveTowards(animator.gameObject.transform.position, player.position, speed * Time.deltaTime);

            }
            else
            {
                am.InvokeAttack("Big Attack_0");
                //animator.SetTrigger("ChargeAttack");
            }
           // animator.SetTrigger("ChargeAttack");
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

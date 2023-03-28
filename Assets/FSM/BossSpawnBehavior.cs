using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnBehavior : StateMachineBehaviour
{
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    private float spawnCounter = 0;
    public float spawnInterval = 100;
    public float numEnemiesSpawned = 0;
    private float maxEnemiesSpawned = 5;
    GameObject enemy;
    Transform spawnerLeft;
    GameObject spawnerRight;
    public float numEnemiesKilled = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // if(spawnerLeft == null || spawnerRight == null)
       // {
         //   Debug.Log("L bozo");
       // }
       // enemy = Instantiate(meleeEnemyPrefab, spawnerLeft.position, Quaternion.identity);  


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if(enemy == null)
        // {
        //     numEnemiesKilled += 1;
        //     if(numEnemiesKilled >= 2)
        //     {
        //         anim.SetTrigger("Death");
        //         Debug.Log("You killed all of the enemies poggy");
        //     }
        //     else
        //     {
        //         enemy = Instantiate(meleeEnemyPrefab, transform.position, transform.rotation);  

        //     }

        // }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}

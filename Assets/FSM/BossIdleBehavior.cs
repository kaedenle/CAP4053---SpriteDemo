using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleBehavior : StateMachineBehaviour
{
    public GameObject Boss;
    public Transform player;
    public float minimumDistance;
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    private float spawnCounter = 0;
    public float spawnInterval = 100;
    public float numEnemiesSpawned = 0;
    private float maxEnemiesSpawned = 5;
    GameObject enemy, enemy2;
    Transform spawnerLeft;
    Transform spawnerRight;
    public float numEnemiesKilled = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("here now");
        spawnerLeft = animator.gameObject.transform.Find("Spawners").Find("SpawnerLeft");
        spawnerRight = animator.gameObject.transform.Find("Spawners").Find("SpawnerRight");

        if(spawnerLeft == null || spawnerRight == null)
        {
            Debug.Log("L bozo");
        }
        if(enemy == null && numEnemiesKilled < 1)
        {
            enemy = Instantiate(meleeEnemyPrefab, spawnerLeft.position, Quaternion.identity);  

        }
        if(enemy2 == null && numEnemiesKilled < 1) 
        {
            enemy2 = Instantiate(meleeEnemyPrefab, spawnerRight.position, Quaternion.identity);
        }
      // player = GameObject.FindGameObjectWithTag("Player").transform;
      // Debug.Log(player.transform.position);
        

        //animator.SetTrigger("Attack");

        Debug.Log("I am here at state enter idle");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(player.transform.position + ", " + animator.rootPosition);
        // Debug.Log(Vector2.Distance(animator.rootPosition, player.transform.position));
        // if(Vector2.Distance(animator.rootPosition, player.transform.position) <= minimumDistance)
        // {
        //   animator.SetTrigger("Attack");
        // }
        //animator.SetTrigger("Attack");

        
        Debug.Log(numEnemiesKilled);
        if(numEnemiesKilled < 1)
        {
            if(enemy == null)
            {
                numEnemiesKilled += 1;
                enemy = Instantiate(meleeEnemyPrefab, spawnerLeft.position, Quaternion.identity);  
            }
            if(enemy2 == null)
            {
                numEnemiesKilled += 1;
                enemy2 = Instantiate(meleeEnemyPrefab, spawnerRight.position, Quaternion.identity);
            }
            
        }
        else
        {
            Destroy(enemy);
            Destroy(enemy2);
            animator.SetTrigger("Phase2");
            Debug.Log("You killed all of the enemies poggy");
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

  
}

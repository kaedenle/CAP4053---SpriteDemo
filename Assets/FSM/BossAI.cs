using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
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
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spawnerLeft = transform.Find("Spawners").Find("SpawnerLeft");
        if(spawnerLeft == null)
        {
            //Debug.Log("L bozo");
        }
        //enemy = Instantiate(meleeEnemyPrefab, spawnerLeft.position, Quaternion.identity);  

    }

    // Update is called once per frame
    void Update()
    {
        
        if(enemy == null)
        {
            numEnemiesKilled += 1;
            if(numEnemiesKilled >= 2)
            {
                //anim.SetTrigger("Death");
                //Debug.Log("You killed all of the enemies poggy");
            }
            else
            {
              //  enemy = Instantiate(meleeEnemyPrefab, transform.position, transform.rotation);  

            }

        }
        // Phase 1
        // if(enemy.health > enemy.health/2)
            // if above half health then phase 1

    }

    void FixedUpdate()
    {
        spawnCounter += 1;
        if(spawnCounter >= spawnInterval)
        {
            spawnCounter = 0;
            //enemy = Instantiate(meleeEnemyPrefab, transform.position, transform.rotation);  
            
        }
    }
}

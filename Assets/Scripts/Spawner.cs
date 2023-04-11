using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Version 2 of Spawn 
// see original at Kaeden's Experiments > Scripts > Spawn
public class Spawner : MonoBehaviour
{
    public GameObject[] enemy;
    public float SpawnRadius = 1.0F;
    [Range(0, 20)] public int minEnemies;
    [Range(0, 20)] public int maxEnemies;
    private int numEnemies;

    public float minimumAcceptablePlayerDistance = 0.5F;
    private int attemptsBeforeAbandom = 5000; // # of random start point generations before the script gives up on the enemy
    
    //should you make new enemies if you walk into the room?
    public bool RespawnOnLoad;
    //should you respawn in same position or reset to original?
    public bool OriginalPos;
    private bool flag = false;
    private static GameObject parent;
    private GameManager gm;
    private GameObject player;

    private BoxCollider2D generateBox;

    // default point to represent null
    private Vector3 NULLPOINT = new Vector3(-1e9F, -1e9F, -1e9F);

    void Awake()
    {
        // grab the boxcollider2d bounding box if there is one
        generateBox = gameObject.GetComponent<BoxCollider2D>();

        if(generateBox != null)
        {
            generateBox.isTrigger = true;
        }

        // check if the enemy # range is invalid
        if(minEnemies > maxEnemies)
        {
            Debug.Log("(Invalid Error) Spawner enemies range invalid");
            minEnemies = maxEnemies;
        }

        numEnemies = Random.Range(minEnemies, maxEnemies + 1);

        // if no enemies are necessary, destroy this
        if(numEnemies <= 0)
            Destroy(gameObject);

        // find the player
        player = GeneralFunctions.GetPlayer();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gm == null) CreateEnemies();
        if (gm != null && !gm.VerifyScene(SceneManager.GetActiveScene().name, RespawnOnLoad)) CreateEnemies();

        // no need for the spawner to continue to take up processing time
        Destroy(gameObject);
    }
    private void CreateEnemies()
    {
        if(enemy == null || enemy.Length == 0)
        {
            Debug.LogError("invalid spawner has no enemies loaded");
            return;
        }
        
        if (flag) return;
        
        if (parent == null)
            parent = GameObject.Find("-- Enemies -- ");
        if(parent == null)
            parent = new GameObject("-- Enemies --");
        
        for(int i = 0; i < numEnemies; i++)
        {
            Debug.Log("Making enemy " + i);
            Vector3 randomPos = GetEnemyPosition();

            if(randomPos == NULLPOINT) continue; // couldn't find an acceptable player distance

            GameObject EnemyType = enemy[Random.Range(0, enemy.Length)];
            if(EnemyType == null)
            {
                Debug.LogError("spawner attempted to spawn null enemy type");
                continue;
            }
            //make new entity
            GameObject enemyObject = Instantiate(EnemyType, randomPos, Quaternion.identity);
            enemyObject.transform.SetParent(parent.transform);

            EnemyStore es = new EnemyStore(enemyObject, EnemyType, randomPos, OriginalPos, EnemyType.GetComponent<HealthTracker>().health);
            if(!RespawnOnLoad) gm.AddEnemy(RespawnOnLoad, es);
            flag = true;
        }
    }

    private Vector3 GetEnemyPosition()
    {
        for(int attempt = 0; attempt < attemptsBeforeAbandom; attempt++)
        {
            Vector3 start = GetRandomStartPosition();

            if(player == null || Vector3.Distance(player.transform.position, start) >= minimumAcceptablePlayerDistance)
            {
                return start;
            }
        }

        return NULLPOINT;
    }

    private Vector3 GetRandomStartPosition()
    {
        if(generateBox == null)
        {
            return Random.insideUnitSphere * SpawnRadius + transform.position;
        }

        else
        {
            return new Vector3(
                Random.Range(generateBox.bounds.min.x, generateBox.bounds.max.x),
                Random.Range(generateBox.bounds.min.y, generateBox.bounds.max.y),
                0
            );
        }
    }

    //draw hitbox
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        //Gizmos.DrawCube(Vector3.zero, transform.localScale);
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
        //Handles.Label(transform.position, EnemyType.name);
        //Gizmos.DrawIcon(transform.position, EnemyType.name, true);

    }
}

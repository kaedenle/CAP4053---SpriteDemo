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
    [Range(0, 100)] public float minPlayerYDistPercent = 25, minPlayerXDistPercent = 25;
    
    public bool RespawnOnLoad; //should you make new enemies if you walk into the room?
    public bool OriginalPos; //should you respawn in same position or reset to original?

    [SerializeField] public LookingBehavior lookingBehavior;
    [Range(0, 100)] public float probOfLookDirection = 100;

    // private internal metrics
    private int numEnemies;
    private float minYPlayer, minXPlayer;
    private int attemptsBeforeAbandom = 5000; // # of random start point generations before the script gives up on the enemy

    // gameobjects and components
    private static GameObject parent;
    private GameObject player;
    private BoxCollider2D generateBox;

    // default point to represent null
    private Vector3 NULLPOINT = new Vector3(-1e9F, -1e9F, -1e9F);

    private List<Base> enemies;
    private string spawner_key;
    void Awake()
    {
        // make sure parent exists
        if (parent == null) parent = GameObject.Find("-- Enemies -- ");
        if(parent == null) parent = new GameObject("-- Enemies --");

        // grab the boxcollider2d bounding box if there is one
        generateBox = gameObject.GetComponent<BoxCollider2D>();
        if(generateBox != null)  generateBox.isTrigger = true;  // make sure the bounding box is not a collider

        // calculate the internal parameters
        minXPlayer = GeneralFunctions.GetCameraWidth() * minPlayerXDistPercent / 100.0F;
        minYPlayer = GeneralFunctions.GetCameraHeight() * minPlayerYDistPercent / 100.0F;

        // check if the enemy # range is invalid
        if(minEnemies > maxEnemies)
        {
            Debug.Log("(Invalid Error) Spawner enemies range invalid");
            minEnemies = maxEnemies;
        }
        numEnemies = Random.Range(minEnemies, maxEnemies + 1);

        // find the player
        player = GeneralFunctions.GetPlayer();

        spawner_key = this.gameObject.name + "_" + ScenesManager.GetCurrentScene().ToString();
        enemies = EntityManager.GetEnemyList(spawner_key);

    }

    void Start()
    {
        // initial spawn of the enemies
        if(enemies != null)
        {
            foreach(Base enemyBase in enemies)
                RecreateEnemy(enemyBase);
        }

        else
        {
            CreateEnemies();
        }
    }

    public string GetKey()
    {
        return spawner_key;
    }
    
    private void CreateEnemies()
    {
        enemies = new List<Base>();

        if(enemy == null || enemy.Length == 0)
        {
            Debug.LogError("invalid spawner has no enemies loaded");
            return;
        }
        
        for(int i = 0; i < numEnemies; i++)
        {
            CreateEnemy();
        }
    }

    // assumes enemies list has been initialized
    // creates single enemy
    public bool CreateEnemy()
    {
        if(enemies.Count >= maxEnemies) return false; // don't go over the count
        // get the starting position
        Vector3 randomPos = GetEnemyPosition();
        if(randomPos == NULLPOINT) return false; // couldn't find an acceptable player distance

        // get the enemy type
        int enemy_type = Random.Range(0, enemy.Length);
        GameObject EnemyType = enemy[enemy_type];
        if(EnemyType == null)
        {
            Debug.LogError("spawner attempted to spawn null enemy type");
            return false;
        }

        // make the enemy
        GameObject enemyObject = Instantiate(EnemyType, randomPos, Quaternion.identity);

        // set enemy configuration
        enemyObject.transform.SetParent(parent.transform);
        SetEnemyDirection(enemyObject);

        // store enemy in list
        enemies.Add(new Base(enemyObject, enemy_type));
        return true;
    }

    public void RecreateEnemy(Base enemyBase)
    {
        //make new entity
        GameObject enemyObject = Instantiate(enemy[enemyBase.spawner_index_type], enemyBase.pos.Get(), Quaternion.identity);
        enemyObject.transform.SetParent(parent.transform);
        enemyBase.SetValues(enemyObject);
    }

    private Vector3 GetEnemyPosition()
    {
        for(int attempt = 0; attempt < attemptsBeforeAbandom; attempt++)
        {
            Vector3 start = GetRandomStartPosition();

            if(player == null || ValidLocation(start))
            {
                return start;
            }
        }

        return NULLPOINT;
    }

    private bool ValidLocation(Vector3 pos)
    {
        return System.Math.Abs(pos.x - player.transform.position.x) >= minXPlayer || 
               System.Math.Abs(pos.y - player.transform.position.y) >= minYPlayer;
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
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }

    public void TriggerSave()
    {
        if(RespawnOnLoad) return; // don't save anything

        enemies.RemoveAll(s => s.entity == null); // remove dead enemies

        foreach(Base b in enemies)
            b.UpdateValues(!OriginalPos);

        enemies.RemoveAll(s => s.health <= 0); // remove dying enemies

        EntityManager.StoreEnemies(enemies, spawner_key);
    }

    /* ====== start Direction Stuff ======= */
    public enum LookingBehavior
    {
        LookTowardPlayer,
        LookLeft
    }

    void SetEnemyDirection(GameObject en)
    {
        bool match = Random.Range(0, 99) < probOfLookDirection;
        bool defLeft = (lookingBehavior == LookingBehavior.LookTowardPlayer ? 
                                        en.transform.position.x >= player.transform.position.x :
                                        true);
        bool lookLeft = (defLeft && match) || (!defLeft && !match);
        Debug.Log("detected that enemy should look left: " + lookLeft + " defLeft=" + defLeft + " match=" + match);
        
        MovementController control = en.GetComponent<MovementController>();
        control.LookingForDirection();
        Vector3 looking = control.looking;

        if(looking == Vector3.left ^ lookLeft)
        {
            control.TurnAround();
            Debug.Log("turning the enemy around");
        }
    }
}

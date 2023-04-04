using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    public GameObject EnemyType;
    public float SpawnRadius;
    public int amount;
    //private GameObject[] EnemyReference;
    
    //should you make new enemies if you walk into the room?
    public bool RespawnOnLoad;
    //should you respawn in same position or reset to original?
    public bool OriginalPos;
    private bool flag = false;
    private static GameObject parent;

    private GameManager gm;
    private void CreateEnemies()
    {
        if (flag) return;
        
        if (parent == null)
            parent = GameObject.Find("-- Enemies -- ");
        if(parent == null)
            parent = new GameObject("-- Enemies --");
        for(int i = 0; i < amount; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * SpawnRadius + transform.position;
            //make new entity
            GameObject enemy = Instantiate(EnemyType, randomPos, Quaternion.identity);
            enemy.transform.SetParent(parent.transform);

            EnemyStore es = new EnemyStore(enemy, EnemyType, randomPos, OriginalPos, EnemyType.GetComponent<HealthTracker>().health);
            if(!RespawnOnLoad) gm.AddEnemy(RespawnOnLoad, es);
            flag = true;
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

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gm == null) CreateEnemies();
        if (gm != null && !gm.VerifyScene(SceneManager.GetActiveScene().name, RespawnOnLoad)) CreateEnemies();
    }
}

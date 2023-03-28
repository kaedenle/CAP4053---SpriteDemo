using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    public GameObject EnemyType;
    public int SpawnRadius;
    public int amount;
    //private GameObject[] EnemyReference;
    
    //should you make new enemies if you walk into the room?
    public bool RespawnOnLoad;
    //should you respawn in same position or reset to original?
    public bool OriginalPos;

    private GameManager gm;
    private void CreateEnemies()
    {
        GameObject parent = GameObject.Find("-- Enemies -- ");
        if (parent == null)
            parent = new GameObject("-- Enemies --");
        for(int i = 0; i < amount; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * SpawnRadius + transform.position;
            //make new entity
            GameObject enemy = Instantiate(EnemyType, randomPos, Quaternion.identity);
            enemy.transform.SetParent(parent.transform);

            EnemyStore es = new EnemyStore(enemy, EnemyType, randomPos, OriginalPos, EnemyType.GetComponent<HealthTracker>().health);
            if(!RespawnOnLoad) gm.AddEnemy(RespawnOnLoad, es);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!gm.VerifyScene(SceneManager.GetActiveScene().name, RespawnOnLoad))
            CreateEnemies();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class WarpManager : MonoBehaviour
{
    public static int WarpNumber = -1;
    private GameObject player;
    public bool KeepPos;
    public GameObject[] warps;
    private bool check = false;
    private GameObject EnemyStore;
    public static IDictionary<string, List<EnemyStore>> EnemyList = new Dictionary<string, List<EnemyStore>>();
    public void SetWarpNum(int num)
    {
        WarpNumber = num;
    }
    public void SaveEnemies(object sender, System.EventArgs e)
    {
        if (!EnemyList.ContainsKey(SceneManager.GetActiveScene().name)) EnemyList.Add(SceneManager.GetActiveScene().name, new List<EnemyStore>());
        else EnemyList[SceneManager.GetActiveScene().name].Clear();
        EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore == null) return;
        foreach(Transform child in EnemyStore.transform)
        {
            CapoScript myScript = child.gameObject.GetComponent<CapoScript>();
            if (myScript == null || !child.gameObject.activeSelf) continue;
            EnemyStore temp = new EnemyStore(child.gameObject, child.gameObject, child.gameObject.transform.position, !KeepPos, child.gameObject.GetComponent<HealthTracker>().healthSystem.getHealth(), myScript.SpawnID);
            EnemyList[SceneManager.GetActiveScene().name].Add(temp);
        }
    }
    public void ReloadEnemies(Scene scene, LoadSceneMode mode)
    {
        if (!EnemyList.ContainsKey(SceneManager.GetActiveScene().name)) return;
        Map1ExtensionManager.Awaken();
        EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore == null) return;
        foreach (EnemyStore es in EnemyList[SceneManager.GetActiveScene().name])
        {
            //go through all children in enemystore. If find ID match set equal
            foreach (Transform child in EnemyStore.transform)
            {
                CapoScript myScript = child.gameObject.GetComponent<CapoScript>();
                if (myScript == null || !child.gameObject.activeSelf) continue;
                if (myScript.SpawnID == es.ID)
                {
                    SetValues(child.gameObject, es);
                    child.GetComponent<NavMeshAgent>().isStopped = true;
                    break;
                }
            }
        }
    }
    private void SetValues(GameObject entity, EnemyStore data)
    {
        //reset health and position as it was
        entity.GetComponent<HealthTracker>().SetHealth(data.Health);
        if (!data.ResetPos) entity.transform.position = data.PosStore;
    }
    void Start()
    {
        ScenesManager.ChangedScenes += SaveEnemies;
        SceneManager.sceneLoaded += ReloadEnemies;
        player = GameObject.Find("Player");
        if(WarpNumber != -1)
        {
            GameObject go = warps[WarpNumber];
            player.transform.position = go.transform.position;
        }
        EnemyStore = GameObject.Find("-- Enemies --");
    }
    
    void Update()
    {
        if (!check)
        {
            if(WarpNumber == -1)
            {
                check = true;
                return;
            }
            GameObject go = warps[WarpNumber];
            check = true;
            WarpTile wt = go.GetComponent<WarpTile>();
            bool flip = false;
            if (wt != null) flip = go.GetComponent<WarpTile>().flip;
            player.GetComponent<Player_Movement>().flipped = flip;
        }
    }
}

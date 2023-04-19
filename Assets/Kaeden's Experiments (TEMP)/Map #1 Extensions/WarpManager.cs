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
    public static IDictionary<string, IDictionary<int, EnemyStore>> EnemyList = new Dictionary<string, IDictionary<int, EnemyStore>>();
    private static bool DeathFlag;
    public void SetWarpNum(int num)
    {
        WarpNumber = num;
    }
    public void SaveEnemies(object sender, System.EventArgs e)
    {
        if (DeathFlag) return;
        if (!EnemyList.ContainsKey(SceneManager.GetActiveScene().name)) EnemyList.Add(SceneManager.GetActiveScene().name, new Dictionary<int, EnemyStore>());
        else EnemyList[SceneManager.GetActiveScene().name].Clear();
        CollapseManager.SaveCollapse();
        EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore == null) return;
        foreach(Transform child in EnemyStore.transform)
        {
            CapoScript myScript = child.gameObject.GetComponent<CapoScript>();
            if (myScript == null || !child.gameObject.activeSelf) continue;
            EnemyStore temp = new EnemyStore(child.gameObject, child.gameObject, child.gameObject.transform.position, !KeepPos, child.gameObject.GetComponent<HealthTracker>().healthSystem.getHealth(), myScript.SpawnID);
            EnemyList[SceneManager.GetActiveScene().name].Add(myScript.SpawnID, temp);
        }
    }
    public void ReloadEnemies(Scene scene, LoadSceneMode mode)
    {
        if (!EnemyList.ContainsKey(SceneManager.GetActiveScene().name)) return;
        Map1ExtensionManager.Awaken();
        CollapseManager.ReloadCollapse();
        EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore == null) return;
        //go through all children in enemystore. If find ID match set equal
        foreach (Transform child in EnemyStore.transform)
        {
            CapoScript myScript = child.gameObject.GetComponent<CapoScript>();
            if (myScript == null || !child.gameObject.activeSelf) continue;
            if (!EnemyList[SceneManager.GetActiveScene().name].ContainsKey(myScript.SpawnID))
            {
                HealthTracker ht = child.gameObject.GetComponent<HealthTracker>();
                if (ht != null) ht.bar.SetActive(false);
                child.gameObject.SetActive(false);
                SetValues(child.gameObject, EnemyList[SceneManager.GetActiveScene().name][myScript.SpawnID]);
                child.GetComponent<NavMeshAgent>().isStopped = true;
            }

        }
        
    }
    private void SetValues(GameObject entity, EnemyStore data)
    {
        //reset health and position as it was
        entity.GetComponent<HealthTracker>().SetHealth(data.Health);
        if (!data.ResetPos) entity.transform.position = data.PosStore;
    }
    public void Clear(object sender, System.EventArgs e)
    {
        foreach (string s in EnemyList.Keys) EnemyList[s].Clear();
        EnemyList.Clear();
        WarpNumber = -1;
        DeathFlag = true;
    }
    public static void Clear()
    {
        foreach (string s in EnemyList.Keys) EnemyList[s].Clear();
        EnemyList.Clear();
    }
    void Start()
    {
        
        player = GameObject.Find("Player");
        if(WarpNumber != -1 && warps.Length != 0)
        {
            GameObject go = warps[WarpNumber];
            player.transform.position = go.transform.position;
        }
        EnemyStore = GameObject.Find("-- Enemies --");
    }
    void OnEnable()
    {
        ScenesManager.ChangedScenes += SaveEnemies;
        SceneManager.sceneLoaded += ReloadEnemies;
        EntityManager.PlayerDead += Clear;
        DeathFlag = false;
    }
    void OnDisable()
    {
        ScenesManager.ChangedScenes -= SaveEnemies;
        SceneManager.sceneLoaded -= ReloadEnemies;
        EntityManager.PlayerDead -= Clear;
    }

    void Update()
    {
        if (!check)
        {
            if(WarpNumber == -1 || warps.Length == 0)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject OneGM;
    //you've completely swapped scenes
    private string SeenScene;
    //you've registered a scene swap but haven't swapped yet
    private ScenesManager.AllScenes SceneID;
    private GameObject player;
    private HealthTracker ht;
    private AttackManager am;
    private Animator anim;
    private int WeaponInt;
    private int Health;
    private int MaxHealth;
    public bool CanSpawnEnemies;
    private bool FirstTime;
    private IDictionary<string, List<EnemyStore>> EnemyLists = new Dictionary<string, List<EnemyStore>>();
    private static GameObject EnemyParent;
    private bool firstLoad = false;
    [HideInInspector]
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (OneGM == null)
            OneGM = gameObject;
        if(OneGM != gameObject)
            Destroy(gameObject);
        //CanSpawnEnemies = false;
    }
    void Start()
    {
        FirstLoad();
    }
    public void FirstLoad()
    {
        if (firstLoad) return;
        firstLoad = true;
        FindStuff();
        UpdateCurrentValues();
        SeenScene = SceneManager.GetActiveScene().name;
        SceneID = ScenesManager.GetCurrentScene();
        MaxHealth = ht.health;
    }
    public void FindStuff()
    {
        player = GameObject.Find("Player");
        if (player == null) return;
        am = player.GetComponent<AttackManager>();
        ht = player.GetComponent<HealthTracker>();
        anim = player.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        //Debug.Log("Entered");
    }
    public void ResetHealth()
    {
        Health = MaxHealth;
        reload();
    }
    public void ResetEquip()
    {
        WeaponInt = -1;
        reload();
    }
    //resets manager, from level to level loading
    public void ResetManager()
    {
        if (!firstLoad) FirstLoad();
        EnemyLists.Clear();
        WeaponInt = -1;
        Health = MaxHealth;
        reload();
    }
    private void reload()
    {
        //find new player object
        if (ht == null || player == null) FindStuff();
        //reset health
        if(ht != null) ht.SetHealth(Health);
        //equip weapon you had in last scene
        if (WeaponInt != -1)
        {
            if(am != null) am.wpnList.index = WeaponInt;
            if(anim != null) anim.SetBool("equiped", true);
        }
        else
        {
            if (anim != null) anim.SetBool("equiped", false);
        }
    }
    private void UpdateCurrentValues()
    {
        Health = ht.healthSystem.getHealth();
        bool equiped = false;
        if(anim != null) equiped = anim.GetBool("equiped");
        WeaponInt = equiped ? am.wpnList.index : -1;
    }

    public void AddEnemy(bool Respawn, EnemyStore enemyStore)
    {
        //if respawn, make new one instead of reusing old one
        if (Respawn) return;
        if (!EnemyLists.ContainsKey(SceneManager.GetActiveScene().name)) return;
        EnemyLists[SceneManager.GetActiveScene().name].Add(enemyStore);
    }
    private void UpdateEnemyValues()
    {
        //if RespawnOnLoad true in spawners, don't reload old enemies
        if (!EnemyLists.ContainsKey(SceneManager.GetActiveScene().name)) return;
        //remove if enemy died
        EnemyLists[SceneManager.GetActiveScene().name].RemoveAll(s => s.entity == null);

        foreach (EnemyStore es in EnemyLists[SceneManager.GetActiveScene().name])
        {
            es.UpdateValues();
        }
    }
    public bool VerifyScene(string scene, bool ignoreresult)
    {
        //ignoreresult allows spawners to respawn even if it isn't its first time being loaded
        bool ret = EnemyLists.ContainsKey(scene);
        //if not in EnemyStore, it's first time and allow spawners to respawn
        if (!ret)
        {
            EnemyLists.Add(scene, new List<EnemyStore>());
            CanSpawnEnemies = false;
            //for multiple spawners in a scewne
            FirstTime = true;
        }
        if (ignoreresult || FirstTime)
        {
            CanSpawnEnemies = false;
            ret = false;
        }
        //if false, spawner respawns
        //if true, manager respawns
        return ret;
    }
    //when game manager respawns entity
    public void ReloadEnemies()
    {
        if (!EnemyLists.ContainsKey(SceneManager.GetActiveScene().name)) return;
        if (EnemyParent == null) EnemyParent = GameObject.Find("-- Enemies -- ");
        //if still null, don't do anything
        if (EnemyParent == null) EnemyParent = new GameObject("-- Enemies --");
        foreach (EnemyStore es in EnemyLists[SceneManager.GetActiveScene().name])
        {
            GameObject enemy = Instantiate(es.entityType, es.PosStore, Quaternion.identity);
            enemy.transform.SetParent(EnemyParent.transform);
            es.SetValues(enemy);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //you've completely swapped scenes
        if(SeenScene != SceneManager.GetActiveScene().name)
        {
            SeenScene = SceneManager.GetActiveScene().name;
            reload();
            //reload enemies here with data
            if (CanSpawnEnemies)
                ReloadEnemies();
            else
                CanSpawnEnemies = true;
            
        }
        else
        {
            UpdateCurrentValues();
        }

        //you've registered a scene swap but haven't done it yet
        if (SceneID != ScenesManager.GetCurrentScene())
        {
            SceneID = ScenesManager.GetCurrentScene();
            //Update enemies values
            UpdateEnemyValues();
            FirstTime = false;
        }
    }
}

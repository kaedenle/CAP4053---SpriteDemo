using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject OneGM;
    private string SeenScene;
    private GameObject player;
    private HealthTracker ht;
    private AttackManager am;
    private Animator anim;
    private int WeaponInt;
    private int Health;
    private int MaxHealth;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (OneGM == null)
            OneGM = gameObject;
        if(OneGM != gameObject)
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        FindStuff();
        UpdateCurrentValues();
        SeenScene = SceneManager.GetActiveScene().name;
        MaxHealth = ht.health;
    }
    public void FindStuff()
    {
        player = GameObject.Find("Player");
        am = player.GetComponent<AttackManager>();
        ht = player.GetComponent<HealthTracker>();
        anim = player.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Debug.Log("Entered");
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
    private void reload()
    {
        //find new player object
        if (ht == null) FindStuff();
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
    // Update is called once per frame
    void Update()
    {
        if(SeenScene != SceneManager.GetActiveScene().name)
        {
            SeenScene = SceneManager.GetActiveScene().name;
            reload();
        }
        else
        {
            UpdateCurrentValues();
        }
    }
}

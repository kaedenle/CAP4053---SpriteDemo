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
    private void reload()
    {
        if (ht == null) FindStuff();
        ht.SetHealth(Health);
        if (WeaponInt != -1)
        {
            am.wpnList.index = WeaponInt;
            anim.SetBool("equiped", true);
        }
    }
    private void UpdateCurrentValues()
    {
        Health = ht.healthSystem.getHealth();
        bool equiped = anim.GetBool("equiped");
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

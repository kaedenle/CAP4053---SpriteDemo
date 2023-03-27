using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject OneGM;
    private ScenesManager.AllScenes SeenScene;
    private GameObject player;
    private HealthTracker ht;
    private WeaponManager wm;
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
        player = GameObject.Find("Player");
        wm = player.GetComponent<WeaponManager>();
        ht = player.GetComponent<HealthTracker>();
        anim = player.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Debug.Log("Entered");
    }
    private void reload()
    {
        //player.GetComponent<HealthTracker>().SetHealth = health;
    }
    private void UpdateCurrentValue()
    {
        Health = ht.health;
    }
    // Update is called once per frame
    void Update()
    {
        if(SeenScene != ScenesManager.GetCurrentScene())
        {
            SeenScene = ScenesManager.GetCurrentScene();
            reload();
        }
        else
        {

        }
    }
}

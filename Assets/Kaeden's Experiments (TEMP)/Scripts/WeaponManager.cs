using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour, IScriptable
{
    public static bool ShortCutCombos = true;
    public static EventHandler Swapping;
    public Sprite[] spriteList;
    public GameObject WeaponUIInstance;
    public SpriteRenderer sr;
    public int weaponID;
    public int BufferWeaponID = 0;
    private Animator animator;
    public bool equiped;
    public AttackManager.WeaponList wpnList = new AttackManager.WeaponList();
    private Hurtbox hrtbx;
    private SpriteRenderer onhand;
    Player_Movement movementScript;
    private WeaponUI ui;
    private AttackManager am;
    [HideInInspector]
    public int prevWeapon;
    private int DisplayedWeapon;
    public float stop_sign_speed;
    public bool weaponDebug;
    public int debuglevel;

    private bool prevFliped = false;

    private IDictionary<InputManager.Keys, int> ComboMappings = new Dictionary<InputManager.Keys, int>
    {
        [InputManager.Keys.Down] = 3,
        [InputManager.Keys.Up] = 2,
        [InputManager.Keys.Right] = 0,
        [InputManager.Keys.Left] = 1
    };
    // Start is called before the first frame update
    void Awake()
    {
        weaponID = 0;
        animator = gameObject.GetComponent<Animator>();
        hrtbx = gameObject?.GetComponent<Hurtbox>();
        onhand = gameObject.transform.Find("Right Arm").Find("On-Hand").GetComponent<SpriteRenderer>();
        movementScript = gameObject.GetComponent<Player_Movement>();
        WeaponUIInstance = GameObject.Find("/-- UI --/Menu Canvas/WeaponUI");
        ui = WeaponUIInstance.GetComponent<WeaponUI>();
        am = GetComponent<AttackManager>();
    }
    void Start()
    {
        int counter = 0;
        wpnList = gameObject.GetComponent<AttackManager>().wpnList;
        while (wpnList.weaponlist[wpnList.index].active == false)
        {
            if (counter > wpnList.weaponlist.Length) break;
            wpnList.index++;
            wpnList.index %= wpnList.weaponlist.Length;
            counter++;
        }
        WeaponLocks();
        InventoryManager.AddedItem += CheckWeapons;
        //constantly check if a weapon is avaliable or not
        ManipulateWeapons();
    }
    private void OnDisable()
    {
        InventoryManager.AddedItem -= CheckWeapons;
    }
    public void CheckWeapons(object o, InventoryManager.AllItems e)
    {
        WeaponLocks();
    }
    //scripts to be disabled/enabled when attacking
    public void ScriptHandler(bool flag)
    {
        this.enabled = flag;
    }
    //what happens when disable
    public void EnableByID(int ID)
    {
        if (ID == 1)
            this.enabled = true;
    }
    public void DisableByID(int ID)
    {
        if (ID == 1)
            this.enabled = false;
    }
    //hard coded weapon access based on level and inventory access
    public void WeaponLocks()
    {
        int additive = 0;
        bool Dagger = true;
        if (Dagger) additive = 8;
        int level = GameData.GetInstance().GetLevel();
        if (weaponDebug) level = debuglevel;
        if(level == 0)
        {
            Calculate(1);
        }
        else if (level == 1)
        {
            if (InventoryManager.HasItem(InventoryManager.AllItems.CastleDagger)) Calculate(3 + additive);
            else Calculate(3);
        }  
        else if (level == 2)
        {
            if (InventoryManager.HasItem(InventoryManager.AllItems.City_Gun)) Calculate(7 + additive);
            else Calculate(3 + additive);
        }
        else if(level == 10) Calculate(15);
        else Calculate(7 + additive);
    }
    public void WeaponLocks(int number)
    {
        Calculate(number);   
    }
    
    private void Calculate(int number)
    {
        int length = am.wpnList.weaponlist.Length;
        for(int i = 0; i < length; i++)
        {
            int working = number >> (i);
            bool result = isKthBitSet(working, 0);
            am.wpnList.weaponlist[i].active = result;
        }
    }
    private bool isKthBitSet(int n, int k)
    {
        if ((n & (1 << k)) > 0)
            return true;
        else
            return false;
    }
    public void SetSprite()
    {
        //set the visual of the weapon from sprite list
        sr.sprite = spriteList[wpnList.index % spriteList.Length];
        DisplayedWeapon = wpnList.index % spriteList.Length;
    }
    private int KeyPressed()
    {
        if (InputManager.DownKeyHold()) return ComboMappings[InputManager.Keys.Down];
        else if (InputManager.UpKeyHold()) return ComboMappings[InputManager.Keys.Up];
        else if (InputManager.RightKeyHold()) return ComboMappings[InputManager.Keys.Right];
        else if (InputManager.LeftKeyHold()) return ComboMappings[InputManager.Keys.Left];
        return -1;
    }
    private void FlipDirections()
    {
        if(prevFliped != movementScript.flipped)
        {
            int temp = ComboMappings[InputManager.Keys.Left];
            ComboMappings[InputManager.Keys.Left] = ComboMappings[InputManager.Keys.Right];
            ComboMappings[InputManager.Keys.Right] = temp;
            prevFliped = movementScript.flipped;
        }
    }
    private void ManipulateWeapons()
    {
        int count = 0;
        while (wpnList.weaponlist[wpnList.index].active == false)
        {
            if (count > spriteList.Length) break;
            wpnList.index += 1;
            wpnList.index %= spriteList.Length;
            count++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        FlipDirections();
        equiped = animator.GetBool("equiped");
        //toggle between not equiped and equiped
        
        if(InputManager.EquipKeyDown() && animator.GetFloat("attack") == 0){
            if (equiped)
            {
                onhand.enabled = false;
                //force UI away
                ui.PreemptivelyFinish();
            }   
            else
            {
                onhand.enabled = true;
                ManipulateWeapons();
                PlayerMetricsManager.IncrementKeeperInt("equip");
            }
                
            animator.SetBool("equiped", !equiped);
            //set animation to follow weapon's ID
            animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
            
        }
        if (InputManager.SwapKeyDown())
        {
            //equip last when when press swap
            if (!equiped)
            {
                onhand.enabled = true;
                PlayerMetricsManager.IncrementKeeperInt("equip");
                animator.SetBool("equiped", !equiped);
                //set animation to follow weapon's ID
                animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
                animator.SetBool("equiped", !equiped);
                ManipulateWeapons();
            }
            //if equiped swap
            if (equiped)
            {
                int original = wpnList.index;
                int count = 0;
                do
                {
                    if (count > spriteList.Length) break;
                    wpnList.index += 1;
                    wpnList.index %= spriteList.Length;
                    count++;
                } while (wpnList.weaponlist[wpnList.index].active == false);
                
                if (WeaponUIInstance != null && !WeaponUIInstance.activeSelf && WeaponUI.render) ui.Invoke();
                else if (WeaponUIInstance != null && WeaponUIInstance.activeSelf && WeaponUI.render) ui.Shift();
                if (wpnList.index != original) PlayerMetricsManager.IncrementKeeperInt("swap");
                 prevWeapon = original;
            }
            
        }
        //change speed if heavy weapon
        if (wpnList.index == 1 && equiped)
            gameObject.GetComponent<Player_Movement>().speed = stop_sign_speed;
        else
            gameObject.GetComponent<Player_Movement>().speed = movementScript.MAX_SPEED;

        //disable weapon from rendering if it's not out  
        if (!equiped)
        {
            sr.sprite = null;
        }
        //input for the player
        if(equiped){
            
            //only swap if not attacking
            if (animator.GetFloat("attack") == 0)
            {
                SetSprite();
                prevWeapon = wpnList.index;
                //set animation to follow weapon's ID
                if (animator.GetFloat("weapon") != wpnList.weaponlist[wpnList.index].ID)
                    animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
            }
            //make it move cancellable
            if (gameObject.GetComponent<Player_Movement>().move_flag && animator.GetFloat("movement") > 0 && animator.GetFloat("attack") > 0){
                gameObject.GetComponent<AttackManager>().DestroyPlay();
                gameObject.GetComponent<Player_Movement>().move_flag = false;
            }
            bool hitStunVar = hrtbx != null ? hrtbx.inHitStun : false;
            //if in hitstun, don't read input (put pausing mechanics in here)
            if (!hitStunVar)
            {
                
                //Swing when press left click
                if (InputManager.Hit1KeyDown() && wpnList.weaponlist[wpnList.index].attack1 != 0)
                {
                    //special case for shooting
                    if(animator.GetFloat("shooting") > 0)
                    {
                        PlayerScript ps = GetComponent<PlayerScript>();
                        if (ps.ShootAgain)
                            ps.cancel = true;
                    }
                    //call attack from integer (defined by Attack blend tree in animator)
                    else if (animator.GetFloat("attack") != 0)
                    {
                        //keyboard shortcuts for combos
                        int press = KeyPressed();
                        if (press != -1 && DisplayedWeapon == wpnList.index && wpnList.weaponlist[press].active && am.HasHitSomething() && ShortCutCombos && am.CancelExists(wpnList.weaponlist[press].attack1))
                        {
                            wpnList.index = press % spriteList.Length;
                        }
                        BufferWeaponID = wpnList.index;
                        am.bufferCancel = wpnList.weaponlist[wpnList.index].attack1;
                    }   
                    else
                        am.InvokeAttack(wpnList.weaponlist[wpnList.index].attack1);
                    
                    //damage self by 5 points
                    //gameObject.GetComponent<HealthTracker>().healthSystem.Damage(5);
                }
                //Swing when press right click
                else if (InputManager.Hit2KeyDown() && wpnList.weaponlist[wpnList.index].attack2 != 0 && animator.GetFloat("shooting") == 0)
                {
                    //call attack from integer (defined by Attack blend tree in animator)
                    if (animator.GetFloat("attack") != 0)
                    {
                        bool dagger = true;
                        //keyboard shortcuts for combos
                        int press = KeyPressed();
                        if (press != -1 && DisplayedWeapon == wpnList.index && wpnList.weaponlist[press].active && am.CanCancel() && ShortCutCombos && am.CancelExists(wpnList.weaponlist[press].attack2))
                        {
                            wpnList.index = press % spriteList.Length;
                            dagger = false;
                        }
                        BufferWeaponID = wpnList.index;
                        am.bufferCancel = wpnList.weaponlist[wpnList.index].attack2;
                        //exception for the dagger
                        if ((animator.GetFloat("attack") == 7 || animator.GetFloat("attack") == 8) && wpnList.weaponlist[wpnList.index].attack2 == 7 && dagger)
                        {
                            am.bufferCancel = (int)(animator.GetFloat("attack") + 1);

                        }
                    }
                    else
                        am.InvokeAttack(wpnList.weaponlist[wpnList.index].attack2);
                }
                if (animator.GetFloat("attack") != 0 && animator.GetBool("Attack") == false)
                {
                    animator.SetTrigger("Attack");
                    PlayerMetricsManager.IncrementKeeperInt("used_" + wpnList.weaponlist[wpnList.index].name);
                }
            }   
        }

        if (animator.GetFloat("attack") == 0 && animator.GetBool("Attack") == true)
            animator.ResetTrigger("Attack");
        
    }

    
}

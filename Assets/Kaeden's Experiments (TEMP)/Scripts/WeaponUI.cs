using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    private GameObject player;
    private WeaponManager wm;
    public AttackManager.WeaponList wpnList;
    private Animator anim;
    private int InternalWeaponID;
    //for ease of access because I don't want to do three child gets
    public GameObject[] tiles;
    public bool Done = false;
    [HideInInspector]
    public static bool render = true;
    private float MAX_TIMER = 1.0f;
    private float timer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        wm = player.transform.root.gameObject?.GetComponent<WeaponManager>();
        anim = gameObject.GetComponent<Animator>();
        wpnList = wm.wpnList;
        InternalWeaponID = wpnList.index;
    }
    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
    private void UpdateWeaponGUI()
    {
        //center
        int count = 0;
        int center = wpnList.index % wm.spriteList.Length;
        while(wpnList.weaponlist[center].active == false)
        {
            if (count > wpnList.weaponlist.Length) break;
            center++;
            center %= wpnList.weaponlist.Length;
            count++;
        }
        tiles[0].GetComponent<Image>().sprite = wm.spriteList[center];

        //left (next)
        count = 0;
        int nex = (wpnList.index + 1) % wm.spriteList.Length;
        while (wpnList.weaponlist[nex].active == false)
        {
            if (count > wpnList.weaponlist.Length) break;
            nex++;
            nex %= wpnList.weaponlist.Length;
            count++;
        }
        tiles[1].GetComponent<Image>().sprite = wm.spriteList[nex];

        //right (prev)
        count = 0;
        int prev = mod((wpnList.index - 1), wm.spriteList.Length);
        while (wpnList.weaponlist[prev].active == false)
        {
            if (count > wpnList.weaponlist.Length) break;
            prev--;
            prev = mod(prev, wpnList.weaponlist.Length);
            count++;
        }
        tiles[2].GetComponent<Image>().sprite = wm.spriteList[prev];

        //next next (for transition)
        count = 0;
        int dummy = (wpnList.index + 2) % wm.spriteList.Length;
        while (wpnList.weaponlist[dummy].active == false)
        {
            if (count > wpnList.weaponlist.Length) break;
            dummy++;
            dummy %= wpnList.weaponlist.Length;
            count++;
        }
        tiles[3].GetComponent<Image>().sprite = wm.spriteList[dummy];
    }
    private void OnEnable()
    {
        wpnList = wm.wpnList;
    }
    public void FinishUp()
    {
        Done = true;
        timer = MAX_TIMER;
        UpdateWeaponGUI();
        InternalWeaponID = wpnList.index;
    }
    public void FinishOut()
    {
        Done = false;
        gameObject.SetActive(false);
    }
    public void PreemptivelyFinish()
    {
        timer = 0;
        gameObject.SetActive(false);
        Done = false;
    }
    public void Invoke()
    {
        gameObject.SetActive(true);
        UpdateWeaponGUI();
        timer = MAX_TIMER;
        anim.Play("PopIn");
    }
    public void Shift()
    {
        gameObject.SetActive(true);
        timer = MAX_TIMER;
        anim.Play("MoveRight");
    }
    //enabler and disabler
    public static void EnableWeaponUI()
    {
        render = true;
    }
    public static void DisableWeaponUI()
    {
        render = false;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            anim.Play("FadeOut");
            Done = false;
        }
        /*if (Done && InternalWeaponID != wpnList.index)
        {
            anim.Play("MoveRight");
            InternalWeaponID = wpnList.index;
            timer = MAX_TIMER;
        }*/
    }
}

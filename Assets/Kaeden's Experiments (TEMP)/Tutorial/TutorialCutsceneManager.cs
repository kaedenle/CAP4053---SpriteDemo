using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCutsceneManager : MonoBehaviour
{
    private GameObject player;
    public GameObject LevelLoaderBlackScreen;
    private LevelLoaderFinish llf;
    private int step = 0;
    public PlayableDirector[] eventList;
    public NPCReport[] dialougeList;
    private GameObject subject;
    private NPCDialogue dialouge;
    private PlayerMetricsManager pmm;
    private AttackManager am;
    private int above;
    private int timesTalked = 0;
    private int pickedup = 0;

    public void Talked(object sender, System.EventArgs e)
    {
        if(sender == (object)subject)
        {
            Debug.Log(subject.name);
            timesTalked++;
        }
    }
    public void PickedUp(object sender, System.EventArgs e)
    {
        Debug.Log("picked");
        pickedup++;
    }

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        am = player.GetComponent<AttackManager>();
        am.wpnList.weaponlist[1].active = false;
        am.wpnList.weaponlist[2].active = false;
        UIManager.DisableHealthUI();
        WeaponUI.DisableWeaponUI();
        llf = LevelLoaderBlackScreen.GetComponent<LevelLoaderFinish>();
        subject = GameObject.Find("Conscious");
        dialouge = subject.GetComponent<NPCDialogue>();
        above = PlayerMetricsManager.GetMetricInt("equip");
        NPCDialogue.Talked += Talked;
        Item.PickedUp += PickedUp;
    }
    public void SetDialouge(NPCReport report)
    {
        dialouge.dialogue = report;
    }
    private void CheckConditions()
    {
        switch (step)
        {
            case 0:
                if(PlayerMetricsManager.GetMetricInt("equip") > above && timesTalked > 1)
                    PlayTimeline();
                break;
            case 1:
                if (PlayerMetricsManager.GetMetricInt("killed") > above)
                    PlayTimeline();
                break;
            case 2:
                if (pickedup > 0)
                    PlayTimeline();
                break;
            case 3:
                if (PlayerMetricsManager.GetMetricInt("swap") > 0)
                    PlayTimeline();
                break;
            case 4:
                if(PlayerMetricsManager.GetMetricInt("used_" + am.wpnList.weaponlist[1].name) > 0)
                    PlayTimeline();
                break;
        }
    }
    private void PlayTimeline()
    {
        if (step >= eventList.Length) return;
        eventList[step].Play();
        step++;
        PresetVar();
    }
    private void PresetVar()
    {
        if (step == 1)
            above = PlayerMetricsManager.GetMetricInt("killed");
    }
    public void EnableSwap()
    {
        player.GetComponent<AttackManager>().wpnList.weaponlist[1].active = true;
    }
    public void Prick()
    {
        //damage the player 5
        player.GetComponent<HealthTracker>().healthSystem.Damage(5);
    }
    //allows events to recognize static calls
    public void DisableCombatUI()
    {
        WeaponUI.DisableWeaponUI();
        UIManager.DisableHealthUI();
    }
    public void EnableCombatUI()
    {
        WeaponUI.EnableWeaponUI();
        UIManager.EnableHealthUI();
    }
    public void DisableHealthUI()
    {
        UIManager.DisableHealthUI();
    }
    public void EnableHealthUI()
    {
        UIManager.EnableHealthUI();
    }
    public void EnableWeaponUI()
    {
        WeaponUI.EnableWeaponUI();
    }
    public void DisableWeaponUI()
    {
        WeaponUI.DisableWeaponUI();
    }
    // Update is called once per frame
    void Update()
    {
        CheckConditions();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

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
    public GameObject dummyObject;
    private DummyUnique dummy;
    private bool Respawn = false;
    public Button Restart;
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
        dummy = dummyObject.GetComponent<DummyUnique>();
    }
    public void SetDialouge(NPCReport report)
    {
        dialouge.dialogue = report;
    }
    private void CheckConditions()
    {
        switch (step)
        {
            //talked and equip
            case 0:
                if(PlayerMetricsManager.GetMetricInt("equip") > above && timesTalked > 1)
                    PlayTimeline();
                break;
            //kill (general)
            case 1:
                if (PlayerMetricsManager.GetMetricInt("killed") > above)
                    PlayTimeline();
                break;
            //pickup item
            case 2:
                if (pickedup > 0)
                    PlayTimeline();
                break;
            //cancel
            case 3:
                if (PlayerMetricsManager.GetMetricInt("cancel") > above && PlayerMetricsManager.GetMetricInt("killed") > above) 
                    PlayTimeline();
                if (dummy.Hide && PlayerMetricsManager.GetMetricInt("cancel") <= above && Respawn)
                    ResetDummy();
                break;
            //swap to sign
            case 4:
                if (PlayerMetricsManager.GetMetricInt("swap") > 0)
                    PlayTimeline();
                if (dummy.Hide && PlayerMetricsManager.GetMetricInt("swap") < 0)
                    ResetDummy();
                break;
            case 5:
                if(PlayerMetricsManager.GetMetricInt("used_" + am.wpnList.weaponlist[1].name) > 0 && PlayerMetricsManager.GetMetricInt("killed") > above)
                    PlayTimeline();
                if (PlayerMetricsManager.GetMetricInt("used_" + am.wpnList.weaponlist[1].name) <= 0 && dummy.Hide)
                    ResetDummy();
                break;
            case 6:
                if (PlayerMetricsManager.GetMetricInt("cross_cancel") > 0 && PlayerMetricsManager.GetMetricInt("killed") > above)
                    PlayTimeline();
                if (PlayerMetricsManager.GetMetricInt("cross_cancel") <= 0 && dummy.Hide)
                    ResetDummy();
                break;
        }
    }
    public void LoadHub()
    {
        ScenesManager.LoadScene(ScenesManager.AllScenes.CentralHub);
    }
    public void EnableRespawn()
    {
        Respawn = true;
    }
    public void DisableRespawn()
    {
        Respawn = false;
    }
    private void ResetDummy()
    {
        dummy.ResetDummy();
        PresetVar();
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
        if (step == 1 || step == 5 || step == 6)
            above = PlayerMetricsManager.GetMetricInt("killed");
        if (step == 3)
            above = PlayerMetricsManager.GetMetricInt("cancel");
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
        Restart.interactable = false;
    }
}

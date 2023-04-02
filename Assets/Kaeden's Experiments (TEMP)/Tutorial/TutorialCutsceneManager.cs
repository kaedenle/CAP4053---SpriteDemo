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
    private int above;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        AttackManager am = player.GetComponent<AttackManager>();
        am.wpnList.weaponlist[1].active = false;
        am.wpnList.weaponlist[2].active = false;
        UIManager.DisableHealthUI();
        WeaponUI.DisableWeaponUI();
        llf = LevelLoaderBlackScreen.GetComponent<LevelLoaderFinish>();
        subject = GameObject.Find("Conscious");
        dialouge = subject.GetComponent<NPCDialogue>();
        pmm = PlayerMetricsManager.GetManager();
        above = pmm.GetMetricInt("equip");
    }
    public void SetDialouge(NPCReport report)
    {
        dialouge.dialogue = report;
    }
    private void CheckConditions()
    {
        if (pmm.GetMetricInt("equip") > above && step == 0)
        {
            PlayTimeline();
        }
        else if(pmm.GetMetricInt("killed") > above && step == 1)
        {
            PlayTimeline();
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
            above = pmm.GetMetricInt("killed");
    }
    // Update is called once per frame
    void Update()
    {
        CheckConditions();
    }
}

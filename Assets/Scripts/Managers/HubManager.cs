using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public enum PhaseTag
    {
        Tutorial,
        Mobster,
        Castle,
        Child,
        Boss
    }

    // array of the starting mind scenes
    private static ScenesManager.AllScenes[] mindSceneStarts =
    {
        ScenesManager.AllScenes.TutorialSkyBox,
        ScenesManager.AllScenes.MobsterRoadDemo,
        ScenesManager.AllScenes.CastleArena,
        ScenesManager.AllScenes.ChildLivingRoom,
        ScenesManager.AllScenes.Boss_Arena
    };

    private bool revisit = false;
    private static int currentPhase;
    public static bool firstTime = false;

    void Awake()
    {
        currentPhase = GameData.GetInstance().GetLevel();
        GameData.GetInstance().SaveCurrentData();
        UIManager.DisableHealthUI();
    }

    void Start()
    {

        if (firstTime)
        {
            GameObject find = GameObject.Find("DialogueOnStartup");
            if (find != null) find.SetActive(false);
        }
        firstTime = true;
        if (currentPhase >= mindSceneStarts.Length)
        {
            LevelManager.TriggerEnd();
        }
    }

    public void Update()
    {
        if(EntityManager.SwapEnabled())
            EntityManager.DisableSwap();
        if(EntityManager.EquipEnabled())
            EntityManager.DisableEquip();
        if(!revisit)
        {
            GameObject GameManager = GameObject.Find("GameManager");
            if (GameManager != null)
            {
                GameManager gm = GameManager.GetComponent<GameManager>();
                //reset gamemanager
                gm.ResetManager();
            }
            revisit = true;
        }
    }

    // load the given phase if you are not loading the phase from the Hub
    public static void LoadPhase(int phase)
    {
        ScenesManager.LoadScene(mindSceneStarts[phase]);
    }
    
    public static void LoadNextMind()
    {
        WeaponUI.EnableWeaponUI();
        // game.IncrementStateAndSave();
        ScenesManager.LoadScene(mindSceneStarts[currentPhase]);
    }
    
    // doesn't distinguish between Demo and Full
    public static bool TagIsCurrent(PhaseTag tag)
    {
        return (int)tag == currentPhase;
    }

    public static void ResetVariables()
    {
        currentPhase = 0;
    }
}

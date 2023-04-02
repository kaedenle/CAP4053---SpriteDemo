using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public enum PhaseTag
    {
        Mobster,
        Child
    }

    // array of the starting mind scenes
    private static ScenesManager.AllScenes[] mindSceneStarts =
    {
        ScenesManager.AllScenes.MobsterRoadDemo,
        ScenesManager.AllScenes.ChildLivingRoom
    };

    // array of starting demo mind scenes
    private static ScenesManager.AllScenes[] demoSceneStarts =
    {
        ScenesManager.AllScenes.MobsterRoadDemo
    };

    private static int currentPhase;
    private static GameState game;

    void Awake()
    {
        game = GameState.LoadGame();
        currentPhase = game.GetLevel();
        UIManager.DisableHealthUI();
    }

    void Start()
    {
        GameObject GameManager = GameObject.Find("GameManager");
        if (GameManager != null)
        {
            GameManager gm = GameManager.GetComponent<GameManager>();
            //reset gamemanager
            gm.ResetManager();
        }
    }

    public void Update()
    {
        if(EntityManager.SwapEnabled())
            EntityManager.DisableSwap();
        if(EntityManager.EquipEnabled())
            EntityManager.DisableEquip();
    }

    // load the given phase if you are not loading the phase from the Hub
    public static void LoadPhase(int phase)
    {
        ScenesManager.LoadScene(mindSceneStarts[phase]);
    }
    
    public static void LoadNextMind()
    {
        game.IncrementStateAndSave();
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

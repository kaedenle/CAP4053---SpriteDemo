using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    private GameObject player;
    private HealthTracker playerHT;
    public enum PhaseTag
    {
        Mobster,
        Child
    }
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerHT = player.GetComponent<HealthTracker>();
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

    private static int currentPhase = 0;

    public void Update()
    {
        if(EntityManager.SwapEnabled())
            EntityManager.DisableSwap();
        if(EntityManager.EquipEnabled())
            EntityManager.DisableEquip();
        //reset player health to max
        if(playerHT.healthSystem.getHealth() != playerHT.health)
            playerHT.SetHealth(playerHT.health);
    }
    
    public static void LoadNextMind()
    {
        currentPhase++;
        ScenesManager.LoadScene(mindSceneStarts[currentPhase - 1]);
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

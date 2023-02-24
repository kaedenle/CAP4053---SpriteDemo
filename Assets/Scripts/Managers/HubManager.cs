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

    private static int currentPhase = 0;
    
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

    // awake is called before start
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("called Start() in HubManager, current phase is " + currentPhase);
    }

    public static void ResetVariables()
    {
        currentPhase = 0;
    }
}

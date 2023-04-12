using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsterLevelManager : SubLevelManager
{
    new void Start()
    {
        startScene = ScenesManager.AllScenes.MobsterRoadDemo;
        SetInstance(this);

        base.Start();
    }

    public override void TriggerReset()
    {
        ResetVariables();
    }

    // reset the manager variables
    public static void ResetVariables()
    {
        
    }
}

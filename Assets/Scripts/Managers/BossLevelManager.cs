using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelManager : SubLevelManager
{
    // delay in seconds after the boss is killed before the level end is triggered
    static float boss_death_wait = 2.5F;

    new void Start()
    {
        if(ScenesManager.GetCurrentScene() == ScenesManager.AllScenes.Boss_BossRoom)
            startScene = ScenesManager.AllScenes.Boss_BossRoom;
        else
            startScene = ScenesManager.AllScenes.Boss_Arena;

        SetInstance(this);
        base.Start();
    }

    public override void TriggerReset()
    {
        ResetVariables();
    }

    new public static void ResetVariables()
    {
        // no variables yet
    }

    public void TriggerBossDeath()
    {        
        StartCoroutine(WaitAndEnd());
    }

    IEnumerator WaitAndEnd()
    {
        yield return new WaitForSeconds(boss_death_wait);
        
        LevelManager.TriggerEnd();
    }

    public static BossLevelManager GetInstance()
    {
        return (BossLevelManager) Instance;
    }
}

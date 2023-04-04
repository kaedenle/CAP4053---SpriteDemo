using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelManager : LevelManager
{
    static BossLevelManager boss_instance;
    // delay in seconds after the boss is killed before the level end is triggered
    static float boss_death_wait = 2.5F;

    new void Awake()
    {
        base.Awake();
        boss_instance = this;
    }

    new void Start()
    {
        setInstance(this, ScenesManager.AllScenes.CastleArena);
        base.Start();
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
        return boss_instance;
    }
}

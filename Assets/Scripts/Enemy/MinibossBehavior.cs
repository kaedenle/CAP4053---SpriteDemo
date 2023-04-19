using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossBehavior : MonoBehaviour
{
    public int bossID;
    private string eventID;

    void Start()
    {
        eventID = "miniboss " + bossID + " death";

        if(LevelManager.GetInteractiveState(eventID))
        {
            //destroy the miniboss
            BackEndDeath();
            Destroy(gameObject);
        }
    }

    public void onDeath()
    {
        Debug.Log("death script test succeeded");
        LevelManager.ToggleInteractiveState(eventID);
    }

    public void BackEndDeath()
    {
        GetComponent<EnemyBase>().DestroyExtraComponents();
        
        IScriptable[] scripts = GetComponents<IScriptable>();
        foreach (IScriptable uni in scripts)
            uni.ScriptHandler(false);

        HealthTracker tracker = GetComponent<HealthTracker>();
        if (tracker.bar.gameObject != null) Destroy(tracker.bar);
        Destroy(gameObject);
    }
}

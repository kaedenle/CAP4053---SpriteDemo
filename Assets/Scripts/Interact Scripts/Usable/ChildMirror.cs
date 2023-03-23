using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMirror : Interactive
{
    public GameObject player;
    bool was_triggered;
    new void Update()
    {
        base.Update();
        was_triggered |= IsTriggered();

        if(was_triggered && !UIActive())
        {
            ChildLevelManager.ToggleMirrorWorld();
            ChildLevelManager.SetRespawn(player.transform.position);
            ScenesManager.ReloadScene();
            was_triggered = false;
        }    
    }
}

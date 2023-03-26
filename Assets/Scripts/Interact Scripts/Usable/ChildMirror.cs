using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMirror : Interactive
{
    new void Update()
    {
        base.Update();

        if(ActivateBehavior())
        {
            Debug.Log("toggling mirror state...");
            ChildLevelManager.ToggleMirrorWorld();
            LevelManager.ReloadScene();
        }    
    }
}

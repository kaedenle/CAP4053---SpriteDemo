using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMirror : Interactive
{
    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        
        Debug.Log("toggling mirror state...");
        ChildLevelManager.ToggleMirrorWorld();
        LevelManager.ReloadScene();
    }
}

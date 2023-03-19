using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLevelManager : LevelManager
{
    // Start is called before the first frame update
    new void Start()
    {
        setInstance(this, ScenesManager.AllScenes.ChildLivingRoom);
        base.Start();
    }
}

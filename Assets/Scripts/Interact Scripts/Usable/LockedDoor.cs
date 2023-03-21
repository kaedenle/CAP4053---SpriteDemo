using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Locked
{
    public ScenesManager.AllScenes nextScene;

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(IsTriggered() && IsUnlocked())
        {
            ScenesManager.LoadScene(nextScene);
        }
    }
}

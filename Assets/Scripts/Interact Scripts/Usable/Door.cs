using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactive
{
    [SerializeField] ScenesManager.AllScenes _nextScene;

    new void Update()
    {
        base.Update();

        if(ActivateBehavior())
        {
            ScenesManager.LoadScene(_nextScene);
        }
    }
}

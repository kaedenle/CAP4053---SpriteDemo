using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactive
{
    [SerializeField] ScenesManager.AllScenes _nextScene;

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();

        ScenesManager.LoadScene(_nextScene);
    }
}

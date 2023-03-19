using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDoorBehavior : Interactive
{
    new void Update()
    {
        if(IsPlayerNear() && InputManager.InteractKeyDown())
        {
            base.Update();
            LevelManager.TriggerEnd();
        }
    }
}

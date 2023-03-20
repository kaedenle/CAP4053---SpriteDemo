using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDoorBehavior : Interactive
{
    new void Start()
    {
        base.Start();
        base.GetOutline().SetFloat("_Outline_Thickness", 7.0F);
    }
    new void Update()
    {
        if(IsPlayerNear() && InputManager.InteractKeyDown())
        {
            base.Update();
            LevelManager.TriggerEnd();
        }
    }
}

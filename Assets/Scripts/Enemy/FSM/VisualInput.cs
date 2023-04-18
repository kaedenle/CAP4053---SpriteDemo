using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/VisualInput")]
    public class VisualInput : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.PlayerVisibile();
        }
    }
}
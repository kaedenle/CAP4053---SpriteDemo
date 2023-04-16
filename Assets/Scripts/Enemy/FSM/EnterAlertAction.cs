using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/EnterAlert")]
    public class EnterAlertAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.TimerComplete = false;
            stateMachine.enemyController.Alerted(stateMachine);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/StopMovement")]
    public class StopMovement : FSMAction
    {        
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.ForceStop();
        }
    }
}
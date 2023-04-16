using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/ChaseAction")]
    public class ChaseAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.Chase();
        }
    }
}
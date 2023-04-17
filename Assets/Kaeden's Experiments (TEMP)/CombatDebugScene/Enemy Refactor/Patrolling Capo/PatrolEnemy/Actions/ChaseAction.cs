using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/Chase")]
    public class ChaseAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.MoveTowards();
        }
    }
}
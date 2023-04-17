using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/Patrol")]
    public class SeekingAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.Patroling();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/EnterChaseAction")]
    public class EnterChaseAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            if(!stateMachine.enemyController.seesPlayer) stateMachine.enemyController.ForceLookPlayer();
        }
    }
}
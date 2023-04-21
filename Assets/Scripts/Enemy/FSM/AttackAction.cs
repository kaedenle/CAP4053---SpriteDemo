using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/AttackAction")]
    public class AttackAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.Attack(stateMachine);
        }
    }
}
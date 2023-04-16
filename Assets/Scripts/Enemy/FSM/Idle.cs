using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/Idle")]
    public class Idle : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.Idle();
        }
    }
}
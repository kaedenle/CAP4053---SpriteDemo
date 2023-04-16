using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/SurpriseAction")]
    public class SurpriseAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.TransitionReady = false;
            stateMachine.enemyController.ExpressSurprise(stateMachine);
        }
    }
}
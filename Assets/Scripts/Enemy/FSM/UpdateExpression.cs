using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/UpdateExpression")]
    public class UpdateExpression : FSMAction
    {
        public string expressionText;
        
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.UpdateExpression(expressionText);
        }
    }
}
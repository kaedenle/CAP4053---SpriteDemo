using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/UpdateReactionTime")]
    public class UpdateReactionTime : FSMAction
    {
        public EnemyStats.SurpriseReactionType reactionType;
        
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.UpdateSurpriseReactionTime(reactionType);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Conditions/InAttackRangeCondition")]
    public class InAttackRangeCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return( stateMachine.enemyController.InRangeOfPlayer());
        }
    }

}
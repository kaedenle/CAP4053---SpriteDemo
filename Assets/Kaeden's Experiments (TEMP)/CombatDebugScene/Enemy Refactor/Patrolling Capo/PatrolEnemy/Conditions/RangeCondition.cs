using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Conditions/RangeCondition")]
    public class RangeCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return stateMachine.enemyController.PlayerInRange();
        }
    }
}
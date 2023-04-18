using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Conditions/DeathCondition")]
    public class DeathCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return stateMachine.ht.healthSystem.getHealth() == 0;
        }
    }
}
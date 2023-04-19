using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Conditions/BeingAttackedCondition")]
    public class BeingAttackedCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return stateMachine.enemyController.BeingAttacked();
        }
    }
}
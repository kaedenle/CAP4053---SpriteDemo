using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Conditions/HurtCondition")]
    public class HurtCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return stateMachine.hb.inHitStun;
        }
    }
}
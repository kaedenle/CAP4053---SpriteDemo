using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Conditions/SeePlayerCondition")]
    public class SeePlayerCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return (stateMachine.enemyController.PlayerVisibile());
        }
    }
}
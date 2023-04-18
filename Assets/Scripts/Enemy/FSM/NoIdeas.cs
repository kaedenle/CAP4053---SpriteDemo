using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Conditions/NoIdeas")]
    public class NoIdeas : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return stateMachine.enemyController.NoIdeas();
        }
    }

}
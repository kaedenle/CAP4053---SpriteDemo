using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Conditions/PlayerDeadCondition")]
    public class PlayerDeadCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            return GameManager.OneGM.GetComponent<GameManager>().GetPlayerHealth() <= 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Conditions/TimerCompleteCondition")]
    public class TimerCompleteCondition : Condition
    {
        public override bool ConditionMet(FSM stateMachine)
        {
            if(stateMachine.displayState) Debug.Log("timer complete: " + stateMachine.TimerComplete);
            return stateMachine.TimerComplete;
        }
    }
}
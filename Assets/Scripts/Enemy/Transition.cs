using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        public Condition Condition;
        public FSMState TrueState;
        public FSMState FalseState;

        public FSMState NewState(FSM stateMachine)
        {
            return Condition.ConditionMet(stateMachine) ? TrueState : FalseState;
        }
    }
}
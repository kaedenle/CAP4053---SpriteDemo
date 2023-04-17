using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatrolEnemy
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool ConditionMet(FSM statemachine);
    }
}
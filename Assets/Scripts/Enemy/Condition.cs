using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool ConditionMet(FSM statemachine);
    }
}
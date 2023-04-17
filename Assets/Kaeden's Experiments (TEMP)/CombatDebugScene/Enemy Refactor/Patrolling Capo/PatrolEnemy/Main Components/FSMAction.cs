using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatrolEnemy
{
    public abstract class FSMAction : ScriptableObject
    {
        public abstract void Execute(FSM stateMachine);
    }
}
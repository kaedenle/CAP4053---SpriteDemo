using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public abstract class FSMAction : ScriptableObject
    {
        public abstract void Execute(FSM stateMachine);
    }
}
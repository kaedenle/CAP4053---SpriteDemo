using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatrolEnemy
{
    public class FSMState : ScriptableObject
    {
        public virtual void Enter(FSM stateMachine) { }
        public virtual void Execute(FSM stateMachine) { }
        public virtual void Exit(FSM stateMachine) {}
    }
}
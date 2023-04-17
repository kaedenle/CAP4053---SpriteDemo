using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public class FSMState : ScriptableObject
    {
        public string stateName;
        public virtual void Enter(FSM stateMachine) { }
        public virtual void Execute(FSM stateMachine) { }
        public virtual void Exit(FSM stateMachine) {}
    }
}
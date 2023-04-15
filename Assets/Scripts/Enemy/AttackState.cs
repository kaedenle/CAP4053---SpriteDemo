using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/States/AttackState")]
    public sealed class AttackState : State
    {
        public override void Execute(FSM stateMachine)
        {
            if(stateMachine.ActionReady)
            {
                base.Execute(stateMachine);
            }
        }

        public override void Enter(FSM stateMachine)
        {
            stateMachine.ActionReady = false;

            foreach (var action in EnterActions)
                action.Execute(stateMachine);
        }

        public override void Exit(FSM stateMachine)
        {
            foreach (var action in ExitActions)
                action.Execute(stateMachine);
        }
    }
}
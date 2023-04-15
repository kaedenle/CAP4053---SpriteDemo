using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/State")]
    public class State : FSMState
    {
        public List<FSMAction> EnterActions = new List<FSMAction>();
        public List<FSMAction> Action = new List<FSMAction>();
        public List<FSMAction> ExitActions = new List<FSMAction>();
        public List<Transition> Transitions = new List<Transition>();

        public override void Execute(FSM stateMachine)
        {
            // execute actions
            foreach (var action in Action)
                action.Execute(stateMachine);

            // try to move to a new state
            foreach(var transition in Transitions)
            {
                FSMState state = transition.NewState(stateMachine);
                if(!(state is RemainInState))
                {
                    stateMachine.ChangeState( state );
                    break; // stop checking for new states
                }
            }
        }

        public override void Enter(FSM stateMachine)
        {
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
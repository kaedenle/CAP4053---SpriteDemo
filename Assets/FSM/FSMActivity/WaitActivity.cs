using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM.Activities
{
    public class WaitActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            //stateMachine.GetComponent<RigidBody2D>.velocity = Vector2.zero;
            stateMachine.GetComponent<Animator>().SetBool("isWalk", false);
        }
        public override void Execute(BaseStateMachine stateMachine)
        {}
        public override void Exit(BaseStateMachine stateMachine) {}
    }

}

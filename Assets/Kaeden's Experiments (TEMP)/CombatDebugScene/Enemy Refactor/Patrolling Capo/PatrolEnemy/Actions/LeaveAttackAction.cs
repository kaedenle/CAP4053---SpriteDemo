using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/LeaveAttackAction")]
    public class LeaveAttackAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.am.DestroyPlay();
            stateMachine.anim.SetBool("Attacking", false);
        }
    }
}
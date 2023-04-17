using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/LeaveHurtAction")]
    public class LeaveHurtAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.myScript.LeaveHitStun();
        }
    }
}
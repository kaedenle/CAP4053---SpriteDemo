using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/EnterAttackAction")]
    public class EnterAttackAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.anim.SetBool("Attacking", true);
            stateMachine.am.InvokeAttack("Attack");
        }
    }
}
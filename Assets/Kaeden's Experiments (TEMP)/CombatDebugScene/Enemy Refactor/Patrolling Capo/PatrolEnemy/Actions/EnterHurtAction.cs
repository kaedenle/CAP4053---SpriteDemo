using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/EnterHurtAction")]
    public class EnterHurtAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.myScript.HitStunAni();
        }
    }
}
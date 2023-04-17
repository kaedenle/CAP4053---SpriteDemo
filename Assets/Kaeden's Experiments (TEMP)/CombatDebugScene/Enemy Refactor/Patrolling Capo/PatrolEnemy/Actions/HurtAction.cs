using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PatrolEnemy
{
    [CreateAssetMenu(menuName = "PatrolEnemy/Actions/HurtAction")]
    public class HurtAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            stateMachine.hb.HitstunTimer();
        }
    }
}
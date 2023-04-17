using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/UpdateFOV")]
    public class UpdateFOV : FSMAction
    {
        public MovementStats.FOVType visionType;
        
        public override void Execute(FSM stateMachine)
        {
            stateMachine.enemyController.UpdateVision(visionType);
        }
    }
}
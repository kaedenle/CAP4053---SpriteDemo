using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System;

namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/TurnAround")]
    public class TurnAround : FSMAction
    {
        private float lastTurned;
        private float turnDelay;
        
        public override void Execute(FSM stateMachine)
        {
            // first turn
            if(lastTurned == 0 && turnDelay == 0)
            {
                lastTurned = Time.time;
                turnDelay = Random.Range(0.2F, 0.5F);
            }

            if(Time.time - lastTurned >= turnDelay)
            {
                stateMachine.enemyController.TurnAround();
                lastTurned = Time.time;
                turnDelay = Random.Range(0.75F, 1.75F);
            }
        }
    }
}
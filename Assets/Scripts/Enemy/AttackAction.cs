using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEnemy
{
    [CreateAssetMenu(menuName = "FSM/Actions/AttackAction")]
    public class AttackAction : FSMAction
    {
        public override void Execute(FSM stateMachine)
        {
            Debug.Log("attacking");
            Attack(stateMachine);
        }

        IEnumerator Attack(FSM stateMachine)
        {
            Debug.Log("got here");
            stateMachine.enemyController.Attack();
            Debug.Log("initial state: " +  stateMachine.enemyController.enabled);
            yield return new WaitUntil(() => stateMachine.enemyController.enabled);
            stateMachine.ActionReady = true;
            Debug.Log("finished attacking");
        }
    }
}
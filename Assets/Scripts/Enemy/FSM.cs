using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemy
{
    public class FSM : MonoBehaviour
    {
        [SerializeField] private FSMState _initialState;
        public FSMState currentState { get; set; }
        public EnemyBase enemyController { get; set; }
        public bool ExecutionReady {get; set;} = true;
        public bool TransitionReady {get; set;} = true;
        public bool TimerComplete {get; set;} = true;

        private void Awake()
        {
            currentState = _initialState;
            enemyController = gameObject.GetComponent<EnemyBase>();
        }

        private void Update()
        {
            currentState.Execute(this);
        } 

        public void ChangeState(FSMState newState)
        {
            if(currentState != null)
                currentState.Exit(this);
            
            currentState = newState;
            currentState.Enter(this);
        }
    }    
}
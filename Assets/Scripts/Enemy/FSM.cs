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
        public bool ExecutionReady {get; set;}

        private void Awake()
        {
            currentState = _initialState;
            enemyController = gameObject.GetComponent<EnemyBase>();
        }

        private void Start()
        {
            if(_initialState != null) _initialState.Enter(this);
        }

        private void Update()
        {
            if(ExecutionReady)
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
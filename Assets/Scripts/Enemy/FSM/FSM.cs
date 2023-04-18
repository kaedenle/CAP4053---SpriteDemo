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

        private bool demo_info = true;
        private float time_of_last_state_change = 0;

        private void Awake()
        {
            currentState = _initialState;
            enemyController = gameObject.GetComponent<EnemyBase>();
            time_of_last_state_change = Time.time;
        }

        // execute the current state (technically it could use a safety feature)
        private void Update()
        {
            currentState.Execute(this);
        } 

        // exit current state and enter new state
        public void ChangeState(FSMState newState)
        {
            if(currentState != null)
                currentState.Exit(this);
            
            currentState = newState;
            time_of_last_state_change = Time.time;
            currentState.Enter(this);
        }

        // checks if the state is the same since the recorded time_check (i.e. no state changes)
        public bool StateConstant(float time_check)
        {
            return time_of_last_state_change <= time_check;
        }

        private void OnGUI()
        {
            if(demo_info)
            {
                string content = currentState != null ? currentState.stateName : "(no current state)";
                GUILayout.Label($"<color='white'><size=40>{content}</size></color>");
            }
        }
    }    
}
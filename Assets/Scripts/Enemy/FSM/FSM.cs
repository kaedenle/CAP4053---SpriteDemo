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

        private void Update()
        {
            currentState.Execute(this);
        } 

        public void ChangeState(FSMState newState)
        {
            if(currentState != null)
                currentState.Exit(this);
            
            currentState = newState;
            time_of_last_state_change = Time.time;
            currentState.Enter(this);
        }

        public bool StateConstant(float time_check)
        {
            if(GeneralFunctions.IsDebug()) Debug.Log("last change: " + time_of_last_state_change + " time to check: " + time_check);
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
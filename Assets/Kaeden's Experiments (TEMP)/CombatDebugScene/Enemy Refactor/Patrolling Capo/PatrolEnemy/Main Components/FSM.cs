using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PatrolEnemy
{
    public class FSM : MonoBehaviour
    {
        [SerializeField] private FSMState _initialState;
        public FSMState currentState { get; set; }
        public Patrol enemyController { get; set; }
        public bool ExecutionReady {get; set;} = true;
        public bool TransitionReady {get; set;} = true;
        public bool TimerComplete {get; set;} = true;
        public Animator anim { get; set; }
        public CapoScript myScript { get; set; }
        public Hurtbox hb { get; set; }
        public AttackManager am { get; set; }

        private void Awake()
        {
            currentState = _initialState;
            enemyController = gameObject.GetComponent<Patrol>();
            enemyController.init();
            anim = gameObject.GetComponent<Animator>();
            myScript = gameObject.GetComponent<CapoScript>();
            hb = gameObject.GetComponent<Hurtbox>();
            am = gameObject.GetComponent<AttackManager>();
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
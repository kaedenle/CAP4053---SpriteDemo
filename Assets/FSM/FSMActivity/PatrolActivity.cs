using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolActivity : Activity
    {
        public float speed = 1; // how fast we should move around while patrolling?
 
        public override void Enter(BaseStateMachine stateMachine)
        {
            var PatrolPoints   = stateMachine.GetComponent<PatrolPoints>();
            var SpriteRenderer = stateMachine.GetComponent<SpriteRenderer>();
            var Animator       = stateMachine.GetComponent<Animator>();
            SpriteRenderer.flipX = (PatrolPoints.GetTargetPointDirection().x > 0) ? true : false;
            Animator.SetBool("isWalk", true);
        }
 
        public override void Execute(BaseStateMachine stateMachine)
        {
            var PatrolPoints   = stateMachine.GetComponent<PatrolPoints>();
            var RigidBody      = stateMachine.GetComponent<Rigidbody2D>();
            float x            = PatrolPoints.GetTargetPointDirection().x;
             
            Vector2 position = RigidBody.position + new Vector2(x * speed * Time.fixedDeltaTime, RigidBody.position.y);
            RigidBody.MovePosition(position);
        }
 
        public override void Exit(BaseStateMachine stateMachine)
        {
            var PatrolPoints = stateMachine.GetComponent<PatrolPoints>();
            PatrolPoints.SetNextTargetPoint();
        }
    }

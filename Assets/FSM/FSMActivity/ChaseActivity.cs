using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseActivity : Activity
    {
        GameObject    target;     // target to chase
        public string targetTag;  // the tag of the target game object we want to check against
        public float  speed = 1;  // how fast we should chase the target?
 
        public override void Enter(BaseStateMachine stateMachine)
        {
            target = GameObject.FindWithTag(targetTag);
            stateMachine.GetComponent<Animator>().SetBool("isWalk", true);
        }
 
        public override void Execute(BaseStateMachine stateMachine)
        {
            var RigidBody = stateMachine.GetComponent<Rigidbody2D>();
            var SpriteRenderer = stateMachine.GetComponent<SpriteRenderer>();
             
            Vector2 dir = (target.transform.position - stateMachine.transform.position).normalized;
            RigidBody.velocity = new Vector2(dir.x * speed, RigidBody.velocity.y);
            SpriteRenderer.flipX = (dir.x > 0) ? true : false;
        }
 
        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }

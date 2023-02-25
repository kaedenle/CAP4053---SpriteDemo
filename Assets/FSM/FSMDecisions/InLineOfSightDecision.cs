using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLineOfSightDecision : Decision
    {
        public LayerMask layerMask;
        public float distanceThreshold = 3f;   // distance threshold to check against; 
        Vector3 prevPosition = Vector3.zero;
        Vector3 prevDir = Vector3.zero;
 
        public override bool Decide(BaseStateMachine stateMachine)
        {
            Vector3 dir      = (stateMachine.transform.position - prevPosition).normalized;
            dir              = (dir.Equals(Vector3.zero)) ? prevDir : dir;
            RaycastHit2D hit = Physics2D.Raycast(stateMachine.transform.position, dir, distanceThreshold, layerMask);
            prevPosition     = stateMachine.transform.position;
            prevDir          = dir;
            return (hit.collider != null) ? true : false;
        }
    }
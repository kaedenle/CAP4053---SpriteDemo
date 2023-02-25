using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDecision : Decision
    {
        GameObject target;                     // target to measure the distance to
        public string targetTag;               // the tag of the target game object we want to check against
        public float distanceThreshold = 3f;   // distance threshold to check against; if distance is greater than or equal to distanceThreshold return true   
        
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (target == null) target = GameObject.FindWithTag(targetTag);
            return (Vector3.Distance(stateMachine.transform.position, target.transform.position) >= distanceThreshold) ? true : false;
        }
    }
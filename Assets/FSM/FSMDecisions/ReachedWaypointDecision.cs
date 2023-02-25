using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachedWaypointDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            return (stateMachine.GetComponent<PatrolPoints>().HasReachedPoint()) ? true : false;
        }
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyBase
{
    protected const string idleAnimateLable = "SlimeIdle";
    
    new void Awake()
    {
        base.Awake();
        expressionOffset = new Vector3(0.2F, 2.35F, 0);
    }

    public override void HitStunAni()
    {
        //TEMPORARY
        animator.Play(idleAnimateLable);
        base.HitStunAni();
    }
}

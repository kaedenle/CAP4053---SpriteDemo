using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyBase
{
    protected const string idleAnimateLable = "SlimeIdle";

    public override void HitStunAni()
    {
        //TEMPORARY
        animator.Play(idleAnimateLable);
        base.HitStunAni();
    }
}

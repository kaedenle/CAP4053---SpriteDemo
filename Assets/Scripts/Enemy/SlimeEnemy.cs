using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyBase
{
    private const string idleAnimateLable = "SlimeIdle";
    public override void EffectManager(string funct)
    {
        base.EffectManager(funct);
    }

    public override void onDeath()
    {
        base.onDeath();
    }

    public override void HitStunAni()
    {
        //TEMPORARY
        animator.Play(idleAnimateLable);
        base.HitStunAni();
    }
}

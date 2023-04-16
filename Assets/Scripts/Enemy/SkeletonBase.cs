using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBase : EnemyBase
{
    public override void MoveAnimation()
    {
        animator.SetFloat("movement", 1);
    }
}

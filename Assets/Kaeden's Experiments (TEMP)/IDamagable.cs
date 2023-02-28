using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void damage(Vector3 knockback, int damage, float hitstun);
}

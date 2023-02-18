using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void damage(float knockback, int damage);
}

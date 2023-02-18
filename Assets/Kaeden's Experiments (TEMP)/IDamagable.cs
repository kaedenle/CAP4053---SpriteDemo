using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void damage(Vector2 knockback, int damage);
}

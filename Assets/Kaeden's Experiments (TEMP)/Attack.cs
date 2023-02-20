using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    //data values
    public int damage;
    public int knockback;
    public int frame;

    //positional values
    public float x_knockback;
    public float y_knockback;
    public float x_pos;
    public float y_pos;
    public float x_scale;
    public float y_scale;

    public int ID;

    public Attack(int damage, int knockback){
        this.damage = damage;
        this.knockback = knockback;
    }
}

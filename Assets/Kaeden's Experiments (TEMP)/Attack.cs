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
    public float hitstun;

    //positional values
    public float x_knockback;
    public float y_knockback;
    public float x_pos;
    public float y_pos;
    public float x_scale;
    public float y_scale;

    public int ID;

    //add two strings SelfInflict and EnemyInflict
    //these two will be called in IUnique as methods
    //Both will be called in attack manager
    //SelfInflict: Function to be called on player
    //EnemyInflict: Funciton to be called on enemy, it will pass in the colliders found in attack manager. If it has a IUnique then it'll run, if not then just ignore these

    public Attack(int damage, int knockback){
        this.damage = damage;
        this.knockback = knockback;
    }
}

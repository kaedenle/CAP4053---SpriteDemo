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
    public Attack()
    {
        this.damage = 0;
        this.knockback = 0;
        this.frame = 0;
        this.hitstun = 0;
        this.x_knockback = 0;
        this.y_knockback = 0;
        this.x_pos = 0;
        this.y_pos = 0;
        this.x_scale = 0;
        this.y_scale = 0;
    }
    public Attack(int damage, int knockback, int frame, float hitstun, float x_knockback, float y_knockback, float x_pos, float y_pos, float x_scale, float y_scale)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.frame = frame;
        this.hitstun = hitstun;
        this.x_knockback = x_knockback;
        this.y_knockback = y_knockback;
        this.x_pos = x_pos;
        this.y_pos = y_pos;
        this.x_scale = x_scale;
        this.y_scale = y_scale;
    }
    public Attack(Attack atk)
    {
        this.damage = atk.damage;
        this.knockback = atk.knockback;
        this.frame = atk.frame;
        this.hitstun = atk.hitstun;
        this.x_knockback = atk.x_knockback;
        this.y_knockback = atk.y_knockback;
        this.x_pos = atk.x_pos;
        this.y_pos = atk.y_pos;
        this.x_scale = atk.x_scale;
        this.y_scale = atk.y_scale;
    }
}

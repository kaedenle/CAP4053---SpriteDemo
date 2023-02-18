using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public int damage;
    public int hitstop;
    public Vector2 knockBack;

    public Attack(int damage, int hitstop, Vector2 knockBack){
        this.damage = damage;
        this.hitstop = hitstop;
        this.knockBack = knockBack;
    }
}

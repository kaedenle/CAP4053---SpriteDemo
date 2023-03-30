using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AttackData
{
    public Vector3 knockback;
    public int damage;
    public float hitstun;
    public float hitstop;
    public int weapon;
    public int attack;
    public AudioClip audio;

    public AttackData(Attack a, Vector3 knockback)
    {
        damage = a.damage;
        hitstun = a.hitstun;
        this.knockback = knockback * a.knockback;
    }
    public AttackData(int damage, float hitstun, Vector3 knockback)
    {
        this.damage = damage;
        this.hitstun = hitstun;
        this.knockback = knockback;
    }
    public void setAux(float hitstop, int weapon, int attack, AudioClip audio)
    {
        this.hitstop = hitstop;
        this.weapon = weapon;
        this.attack = attack;
        this.audio = audio;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem
{
    private int healthAmount;
    private int healthAmountMax;

    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;

    public HealthSystem(int health){
        healthAmountMax = health;
        healthAmount = health;
    }

    public void Damage(int amount){
        healthAmount -= amount;
        if(healthAmount < 0){
            healthAmount = 0;
        }
        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
    }
    public void Set(int amount)
    {
        healthAmount = amount;
        if (healthAmount > healthAmountMax)
            healthAmount = healthAmountMax;
        if (healthAmount < 0)
            healthAmount = 0;
    }
    public void Heal(int amount){
        healthAmount += amount;
        if (healthAmount > healthAmountMax) {
            healthAmount = healthAmountMax;
        }
        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public int getHealth(){
        return healthAmount;
    }

    public float GetHealthNormalized(){
        return (float)healthAmount / healthAmountMax;
    }
}

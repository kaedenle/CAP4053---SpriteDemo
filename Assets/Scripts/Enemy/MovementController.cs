using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IScriptable
{
    private const float ATTACK_TIMER_MAX = 0.0f;
    private float attackTimer;

    /* IScriptable Functions */
    public virtual void EnableByID(int ID)
    {
        if(ID == 0)
            this.enabled = true;
    }

    public virtual void DisableByID(int ID)
    {
        if (ID == 0)
            this.enabled = false;
    }

    //enable and disable script
    public void ScriptHandler(bool flag){
        if(flag){
            attackTimer = ATTACK_TIMER_MAX;
        }
        this.enabled = flag;
    }

    public void Attack()
    {
        GetComponent<AttackManager>().InvokeAttack("SlimeAttack");
    }
}

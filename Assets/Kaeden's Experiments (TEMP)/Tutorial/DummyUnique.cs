using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnique : MonoBehaviour, IUnique
{
    private Animator anim;
    public bool Hide = false;
    private bool CombatReady = true;
    private float timer;
    private float MAX_DEATH_TIMER = 1.75f;
    private bool DeathFlag = false;
    private HealthTracker ht;
    private Rigidbody2D body;
    private EffectsManager fxm;
    public void EffectManager(string s)
    {}
    private void ShakeEntrance()
    {
        if (fxm != null) fxm.ShakeCam(0.1f, 0.5f);
    }
    public void ResetDummy()
    {
        gameObject.SetActive(true);
        HealSelf();
        EnableCombat();
        EnableDamage();
        DeathFlag = false;
        Hide = false;
        transform.position = new Vector3(-4.93f, 20, 0);
        anim.Play("Entrance");
    }
    public void ResetItemDrop()
    {
        GetComponent<ItemDrop>().items[0].percentChance = 0;
    }
    public void HealSelf()
    {
        gameObject.GetComponent<HealthTracker>().healthSystem.Heal(1000);
        ht.deathFlag = false;
        DeathFlag = false;
        anim.Play("DIdle");
    }
    public void ResetPos()
    {
        transform.position = new Vector3(-4.93f, 1.02f, 0);
    }
    public void EnableSelf()
    {
        anim.Play("DIdle");
        Hide = false;
        DeathFlag = false;
        gameObject.GetComponent<HealthTracker>().healthSystem.Heal(1000);
        body.velocity = Vector2.zero;
    }
    public void DisableDamage()
    {
        ht.deathFlag = true;
        body.isKinematic = true;
    }
    public void EnableDamage()
    {
        ht.deathFlag = false;
        body.isKinematic = false;
    }
    public void EnableCombat()
    {
        transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
        CombatReady = true;
    }
    public void DisableCombat()
    {
        transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        CombatReady = false;
    }
    public void onDeath()
    {
        //play death animation
        anim.Play("DDeath");
        gameObject.GetComponent<ItemDrop>().AttemptDrop();
    }
    public void HitStunAni()
    {}
    // Start is called before the first frame update
    
    public void AnimatorPlayerDeath()
    {
        //set flag to freeze game and pull up Death UI
        timer = MAX_DEATH_TIMER;
        DeathFlag = true;
        ResetItemDrop();
    }
    public void HideObj()
    {
        gameObject.transform.position = new Vector3(0, 1000, 0);
        anim.Play("DIdle");
    }
    void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        ht = gameObject.GetComponent<HealthTracker>();
        anim = gameObject.GetComponent<Animator>();
        fxm = FindObjectOfType<EffectsManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (DeathFlag && timer <= 0)
        {
            DisableCombat();
            Hide = true;
        }
    }
    void LateUpdate()
    {
        
        if (Hide) HideObj();
        if (transform.GetChild(1).GetComponent<BoxCollider2D>().enabled != CombatReady) transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = CombatReady;
    }

}

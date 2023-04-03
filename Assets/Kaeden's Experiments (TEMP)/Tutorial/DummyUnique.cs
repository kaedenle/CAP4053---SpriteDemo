using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnique : MonoBehaviour, IUnique
{
    private Animator anim;
    private bool Hide = false;
    public void EffectManager(string s)
    {}
    public void EnableSelf()
    {
        anim.Play("CIdle");
        Hide = false;
    }
    public void DisableDeath()
    {
        this.enabled = false;
    }
    public void EnableDeath()
    {
        this.enabled = true;
    }
    public void EnableCombat()
    {
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
    }
    public void DisableCombat()
    {
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }
    public void onDeath()
    {
        //play death animation
        anim.Play("Death");
        gameObject.GetComponent<ItemDrop>().AttemptDrop();
    }
    public void HitStunAni()
    {}
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    public void AnimatorPlayerDeath()
    {
        //set flag to freeze game and pull up Death UI
        StartCoroutine(WaitThenKill());
    }
    IEnumerator WaitThenKill()
    {
        yield return new WaitForSeconds(2f);
        DisableCombat();
        gameObject.GetComponent<HealthTracker>().healthSystem.Heal(1000);
        Hide = true;
    }
    public void HideObj()
    {
        gameObject.transform.position = new Vector3(0, 1000, 0);
        anim.Play("CIdle");
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Hide)
            HideObj();
    }

}

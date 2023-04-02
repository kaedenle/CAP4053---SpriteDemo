using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnique : MonoBehaviour, IUnique
{
    private Animator anim;
    public void EffectManager(string s)
    {}
    public void EnableCombat()
    {
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        UIManager.EnableHealthUI();
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
        Destroy(gameObject.GetComponent<HealthTracker>().bar);
        Destroy(gameObject);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

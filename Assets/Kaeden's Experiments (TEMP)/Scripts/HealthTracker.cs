using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
[Serializable]

public class HealthTracker : MonoBehaviour, IDamagable
{
    public int health;

    public GameObject bar;
    public Vector3 barOffset;
    private Image damagedBarImage;
    private Image HPBar;
    private bool deathFlag = false;

    private float hitstunaddTimer = 0;
    private const float DAMAGED_TIMER_SHRINK_MAX = 0.5f;
    private float damagedHealthShrinkTimer;

    [HideInInspector] 
    public HealthSystem healthSystem;

    // Starts whether or not this script is enabled
    void Awake()
    {
        GameObject Parent = GameObject.Find("UI Canvas");
        if(Parent == null){
            GameObject UIBar = Resources.Load("Prefabs/UI Canvas") as GameObject;
            Parent = Instantiate(UIBar);
            Parent.name = "UI Canvas";
        }
        //Add personal Healthbar to UI Canvas if it doesn't exist and update follow target
        if(bar.transform.parent != Parent){
            GameObject newBar = Instantiate(bar) as GameObject;
            newBar.transform.SetParent(Parent.transform);

            FollowTarget ft = newBar.GetComponent<FollowTarget>();
            ft.target = gameObject;
            ft.offset = barOffset;
            bar = newBar;
        }
        if (!UIManager.getHealthUI()) bar.SetActive(false);
        HPBar = bar.transform.Find("Foreground").GetComponent<Image>();
        damagedBarImage = bar.transform.Find("Damaged").GetComponent<Image>();
        healthSystem = new HealthSystem(health);
        SetHealthBar(healthSystem.GetHealthNormalized());

        //subscribe to event system
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        damagedBarImage.fillAmount = HPBar.fillAmount;

    }
    public float TotalDown()
    {
        return damagedBarImage.fillAmount;
    }
    // Update is called once per frame
    void Update()
    {
        //health bar effect
        damagedHealthShrinkTimer -= Time.deltaTime;
        if(damagedHealthShrinkTimer < 0 && bar != null){
            if(HPBar.fillAmount < damagedBarImage.fillAmount){
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
                //once grey bar decreases
                if(damagedBarImage.fillAmount == 0){
                    
                }
            }
        }
        //check for death
        if(healthSystem.getHealth() == 0 && !deathFlag){
            //Debug.Log(gameObject.name + " is dead");
            deathFlag = true;
            IUnique unique = gameObject?.GetComponent<IUnique>();
            if (unique != null) unique.onDeath();
        }

        //healing effect

    }
    private void SetHealthBar(float health){
        HPBar.fillAmount = health;
    }
    public void SetHealth(int health)
    {
        healthSystem.Set(health);
        SetHealthBar(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = HPBar.fillAmount;
    }
    public float GetTrueFillAmount()
    {
        return damagedBarImage.fillAmount;
    }
    //implement IDamagable interface
    public void damage(AttackData ad){
        if(!deathFlag){
            hitstunaddTimer = ad.hitstun;
            //Debug.Log(gameObject.name + " took " + ad.damage + " damage and " + ad.knockback + " knockback");
            healthSystem.Damage(ad.damage);
        }
    }

    //events to happen when healed
    private void HealthSystem_OnHealed(object sender, System.EventArgs e){
        SetHealthBar(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = HPBar.fillAmount;
    }
    //events to happen when damage is taken
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e){
        if(!deathFlag)
            damagedHealthShrinkTimer = DAMAGED_TIMER_SHRINK_MAX + hitstunaddTimer;
        SetHealthBar(healthSystem.GetHealthNormalized());
    }
}

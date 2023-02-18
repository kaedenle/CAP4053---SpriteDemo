using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthTracker : MonoBehaviour, IDamagable
{
    public int health;

    public GameObject bar;
    public GameObject prefabUICanvas;
    public Vector3 barOffset;
    private Image damagedBarImage;
    private Image HPBar;
    private bool deathFlag = false;

    private const float DAMAGED_TIMER_SHRINK_MAX = 0.5f;
    private float damagedHealthShrinkTimer;

    [HideInInspector] 
    public HealthSystem healthSystem;

    // Starts whether or not this script is enabled
    void Awake()
    {
        GameObject Parent = GameObject.Find("UI Canvas");
        //Add personal Healthbar to UI Canvas if it doesn't exist and update follow target
        if(bar.transform.parent != Parent){
            GameObject newBar = Instantiate(bar) as GameObject;
            newBar.transform.SetParent(Parent.transform);
            newBar.GetComponent<FollowTarget>().target = gameObject;
            newBar.GetComponent<FollowTarget>().offset = barOffset; 
            bar = newBar;
        }

    }

    void Start(){
        HPBar = bar.transform.Find("Foreground").GetComponent<Image>();
        damagedBarImage = bar.transform.Find("Damaged").GetComponent<Image>();
        healthSystem = new HealthSystem(health);
        SetHealth(healthSystem.GetHealthNormalized());

        //subscribe to event system
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        damagedBarImage.fillAmount = HPBar.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //health bar effect
        damagedHealthShrinkTimer -= Time.deltaTime;
        if(damagedHealthShrinkTimer < 0){
            if(HPBar.fillAmount < damagedBarImage.fillAmount){
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
                //once grey bar decreases
                if(damagedBarImage.fillAmount == 0){
                    //gameObject.GetComponent<HealthTracker>().enabled = false;
                }
            }
        }
        //check for death
        if(healthSystem.getHealth() == 0 && !deathFlag){
            Debug.Log(gameObject.name + " is dead");
            deathFlag = true;
        }
    }
    void SetHealth(float health){
        HPBar.fillAmount = health;
    }

    //implement IDamagable interface
    public void damage(Vector2 knockback, int damage){
        if(!deathFlag){
            Debug.Log(gameObject.name + " took " + damage + " damage and " + knockback + " knockback");
            healthSystem.Damage(damage);
        }
    }

    //events to happen when healed
    private void HealthSystem_OnHealed(object sender, System.EventArgs e){
        SetHealth(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = HPBar.fillAmount;
    }
    //events to happen when damage is taken
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e){
        if(!deathFlag)
            damagedHealthShrinkTimer = DAMAGED_TIMER_SHRINK_MAX;
        SetHealth(healthSystem.GetHealthNormalized());
    }
}

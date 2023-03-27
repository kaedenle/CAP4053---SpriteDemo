using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterEyeBehavior : MonoBehaviour
{
    public bool startOn;
    private SpriteRenderer spriteRenderer;


    private double prob_on = 0.001;  // between 0 and 100
    private double prob_off = 0.003; // between 0 and 100
    private double prob_blink = 0.05;
    private System.Random rand;

    private double blink_time = 2.0;

    private bool on;
    private bool blinking;


    void Awake()
    {
        blinking = false;
        on = startOn;

        rand = new System.Random(Guid.NewGuid().GetHashCode());

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = on;
    }

    // Update is called once per frame
    void Update()
    {
        if(blinking) return;

        double value = rand.NextDouble();

        if(on)
        {
            if(value < (prob_off / 100.0))
            {
                Toggle();
            }

            else if(rand.NextDouble() < ((prob_blink) / 100.0))
                StartCoroutine(Blink());
        }
        else
        {   
            if(value < (prob_on / 100.0))
            {
                Toggle();
            }
        }

        
    }

    void Toggle()
    {
        spriteRenderer.enabled = !on;
        on = !on;
    }

    IEnumerator Blink()
    {
        blinking = true;
        spriteRenderer.enabled = !on;
        yield return new WaitForSecondsRealtime((float)blink_time);
        spriteRenderer.enabled = on;
        blinking = false;
    }
}

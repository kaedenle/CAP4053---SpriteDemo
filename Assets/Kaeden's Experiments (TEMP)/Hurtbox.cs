using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public IDamagable[] damagableScripts;
    // Start is called before the first frame update
    void Start()
    {
        damagableScripts = gameObject.GetComponentsInChildren<IDamagable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

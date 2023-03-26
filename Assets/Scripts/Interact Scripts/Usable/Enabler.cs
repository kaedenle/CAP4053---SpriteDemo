using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : Interactive
{
    public string stateID;
    public GameObject[] enables;
    public bool destroyOnUse = false;

    private bool state = false;


    new void Start()
    {
        state = LevelManager.GetInteractiveState(stateID);
        if(state)
        {
            ActivateChildren();
        }

        base.Start(); 
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if(!state && ActivateBehavior())
        {
            ActivateChildren();
            LevelManager.ToggleInteractiveState(stateID);
            state = true;
        }
    }

    void ActivateChildren()
    {
        foreach(GameObject obj in enables)
        {
            if(obj == null) continue;

            obj.SetActive(true);
        }

        if(destroyOnUse)
            Destroy(gameObject);
    }
}

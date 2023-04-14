using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : Interactive
{

    public string eventID;
    public string stateID;
    public GameObject[] enables;
    public bool destroyOnUse = false;

    private bool state = false;
    public bool gainItem = false;
    [SerializeField] public InventoryManager.AllItems itemGained;
    private bool activated;


    new void Start()
    {
        state = LevelManager.GetInteractiveState(stateID);
        if(state)
        {
            ActivateChildren();
        }

        base.Start(); 
    }

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();

        if(!state)
        {
            ActivateChildren();
            LevelManager.ToggleInteractiveState(stateID);
            state = true;
        }
        if(!activated)
        {
            if(gainItem)
                InventoryManager.AddItem(itemGained);
            
            Activate();

            LevelManager.ToggleInteractiveState(eventID);
            activated = true;

            if(destroyOnUse)
            {
                Destroy(gameObject);
            }
        }
    }

    void Activate()
    {
        foreach(GameObject obj in enables)
        {
            if(obj == null) continue;

            obj.SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTriggersEvent : Interactive
{
    public string eventID;
    public bool destroyOnUse = false;
    public GameObject[] enables;
    public bool gainItem = false;
    [SerializeField] public InventoryManager.AllItems itemGained;

    private bool activated;

    new void Start()
    {
        activated = LevelManager.GetInteractiveState(eventID);

        if(activated)
        {
            Activate();

            if(destroyOnUse)
            {
                Destroy(gameObject);
            }
        }

        base.Start();
    }

    new void Update()
    {
        base.Update();

        if(!activated && ActivateBehavior())
        {
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
        if(gainItem)
        {
            InventoryManager.AddItem(itemGained);
        }

        foreach(GameObject obj in enables)
        {
            if(obj == null) continue;

            obj.SetActive(true);
        }        
    }

}

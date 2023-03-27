using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTriggersEvent : NPCDialogue
{
    public string triggeredEventId;
    public bool destroyOnTrigger = false;
    public bool gainItem = false;
    [SerializeField] public InventoryManager.AllItems itemGained;
    public GameObject[] enables;


    private bool activated = false;

    new void Start()
    {
        activated = LevelManager.GetInteractiveState(triggeredEventId);

        if(activated)
        {
            ActivateChildren();

            if(destroyOnTrigger)
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
            if(gainItem)
            {
                InventoryManager.AddItem(itemGained);
            }

            ActivateChildren();

            if(destroyOnTrigger)
            {
                Destroy(gameObject);
            }

            LevelManager.ToggleInteractiveState(triggeredEventId);
            activated = true;
        }
    }

    void ActivateChildren()
    {
        foreach(GameObject obj in enables)
        {
            if(obj == null) continue;

            obj.SetActive(true);
        }
    }
}

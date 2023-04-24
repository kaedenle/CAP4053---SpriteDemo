using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCTriggersEvent : NPCDialogue
{
    public string triggeredEventId;
    public bool destroyOnTrigger = false;
    public bool gainItem = false;
    [SerializeField] public InventoryManager.AllItems itemGained;
    public GameObject[] enables;
    public UnityEvent m_MyEvent;



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
            LevelManager.ToggleInteractiveState(triggeredEventId);
            activated = true;

            if(m_MyEvent != null)
                m_MyEvent.Invoke();

            if(destroyOnTrigger)
            {
                Destroy(gameObject);
            }
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

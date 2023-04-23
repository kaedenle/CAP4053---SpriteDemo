using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemEvent : Item
{
    public UnityEvent m_MyEvent;

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        m_MyEvent.Invoke();
    }
}

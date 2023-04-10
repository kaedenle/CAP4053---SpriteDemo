using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorEvent : Interactive
{
    [SerializeField] ScenesManager.AllScenes _nextScene;
    public UnityEvent m_MyEvent;
    new void Update()
    {
        base.Update();

        if(ActivateBehavior())
        {
            m_MyEvent.Invoke();
            ScenesManager.LoadScene(_nextScene);
        }
    }
}

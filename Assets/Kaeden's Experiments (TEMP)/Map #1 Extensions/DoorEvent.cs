using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorEvent : Interactive
{
    [SerializeField] ScenesManager.AllScenes _nextScene;
    public UnityEvent m_MyEvent;

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        m_MyEvent.Invoke();
        ScenesManager.LoadScene(_nextScene);
    }
    public void ForceNextScene()
    {
        m_MyEvent.Invoke();
        ScenesManager.LoadScene(_nextScene);
    }
}

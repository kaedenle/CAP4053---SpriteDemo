using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaleighDiary : Interactive
{
    public GameObject DiaryUI;
    private bool delayFinished = false;
    private float delay_time = 0.1F;

    new void Update()
    {
        base.Update();

        if(delayFinished && InputManager.ContinueKeyPressed())
        {
            DiaryUI.SetActive(false);
            EntityManager.Unpause();
        }
    }
    
    protected override void ActivateBehaviors()
    {
        // activate sounds
        base.ActivateBehaviors();

        // activate UI
        DiaryUI.SetActive(true);

        // gain item
        InventoryManager.AddItem(InventoryManager.AllItems.ReadChildDiary);

        // input buffer
        StartCoroutine(StartTimer());

        // backend pause
        EntityManager.DialoguePause();
    }

    IEnumerator StartTimer()
    {
        delayFinished = false;
        yield return new WaitForSecondsRealtime(delay_time);
        delayFinished = true;
    }
}

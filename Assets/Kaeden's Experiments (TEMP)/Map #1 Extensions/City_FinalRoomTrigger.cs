using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class City_FinalRoomTrigger : MonoBehaviour
{
    public bool flag = false;
    public bool gunflag = false;
    public PlayableDirector final;
    public PlayableDirector guntutorial;
    void OnEnable()
    {
        InventoryManager.AddedItem += ExecuteEvent;
    }
    private void OnDisable()
    {
        InventoryManager.AddedItem -= ExecuteEvent;
    }
    public void ExecuteEvent(object sender, InventoryManager.AllItems e)
    {
        //when you have gun, alleykey, and syringe
        if (InventoryManager.HasItem(InventoryManager.AllItems.City_Syringe)
            && InventoryManager.HasItem(InventoryManager.AllItems.City_Gun)
            && InventoryManager.HasItem(InventoryManager.AllItems.City_AlleyKey)
            && !flag)
        {
            flag = true;
            if (final != null) final.Play();
        }
        if(!gunflag && InventoryManager.HasItem(InventoryManager.AllItems.City_Gun))
        {
            if (guntutorial != null) guntutorial.Play();
        }
            
    }
}

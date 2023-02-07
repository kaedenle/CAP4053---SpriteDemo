using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems _requiredKey;
    [SerializeField] ScenesManager.AllScenes _nextScene;

    public void OnTriggerEnter2D()
    {
        // opens the gate and loads the alley scene if the player has the key
        if(MobsterLevelManager.UnlockAlleyGate(_requiredKey))
        {
            ScenesManager.LoadScene(_nextScene);
        }
    }

}

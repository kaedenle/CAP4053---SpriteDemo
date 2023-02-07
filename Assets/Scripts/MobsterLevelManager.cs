using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsterLevelManager : MonoBehaviour
{
    static bool _alleyGateOpen = false;

    // Start is called before the first frame update
    public static bool AlleyGateUnlocked()
    {
        return _alleyGateOpen;
    }

    // returns true if the gate is already unlocked or if the player has the key
    public static bool UnlockAlleyGate(InventoryManager.AllItems key)
    {
        if (AlleyGateUnlocked()) return true;

        if(InventoryManager.HasItem(key))
        {
            Debug.Log("Alley Gate Has Been Opened");
            _alleyGateOpen = true;
            return true;
        }

        return false;
    }
}

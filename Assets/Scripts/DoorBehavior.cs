using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems _requiredKey;
    [SerializeField] ScenesManager.AllScenes _nextScene;
    private bool near = true;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
            near = true;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player")
            near = false;
    }

    public void Update()
    {
        // opens the gate and loads the alley scene if the player has the key
        if(near && InputManager.InteractKeyDown() && MobsterLevelManager.UnlockAlleyGate(_requiredKey))
        {
            ScenesManager.LoadScene(_nextScene);
        }
    }

}

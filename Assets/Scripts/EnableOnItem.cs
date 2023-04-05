using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnItem : MonoBehaviour
{
    public InventoryManager.AllItems requiredItem;
    public GameObject[] enables;
    public bool itemInScene = true;

    // Start is called before the first frame update
    void Awake()
    {
        if(InventoryManager.PickedUp(requiredItem))
        {
            EnableItems();
        }

        else if(!itemInScene)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(itemInScene && InventoryManager.PickedUp(requiredItem))
        {
            EnableItems();
        }
    }

    void EnableItems()
    {
        foreach(GameObject obj in enables)
        {
            if(obj != null)
                obj.SetActive(true);
        }

        Destroy(gameObject);
    }
}

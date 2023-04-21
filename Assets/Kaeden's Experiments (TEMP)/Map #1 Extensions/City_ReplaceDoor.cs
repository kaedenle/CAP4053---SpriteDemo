using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_ReplaceDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite newDoor;
    private SpriteRenderer sr;
    public void ExecuteAllPages(object sender, InventoryManager.AllItems e)
    {
        if (AntonioManager.CheckPapers() >= 8) sr.sprite = newDoor;
    }
    void OnEnable()
    {
        InventoryManager.AddedItem += ExecuteAllPages;
        sr = GetComponent<SpriteRenderer>();
        ExecuteAllPages(null, InventoryManager.AllItems.City_Paper1);
    }
    private void OnDisable()
    {
        InventoryManager.AddedItem -= ExecuteAllPages;
    }
}

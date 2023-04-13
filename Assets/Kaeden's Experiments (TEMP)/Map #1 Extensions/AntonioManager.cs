using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonioManager : MonoBehaviour
{
    [HideInInspector]
    public int papers = 0;
    //will check to see if you picked up paper
    private HashSet<InventoryManager.AllItems> checkPaper = new HashSet<InventoryManager.AllItems>() { InventoryManager.AllItems.City_Paper1, InventoryManager.AllItems.City_Paper2, 
        InventoryManager.AllItems.City_Paper3, InventoryManager.AllItems.City_Paper4, InventoryManager.AllItems.City_Paper5, InventoryManager.AllItems.City_Paper6, InventoryManager.AllItems.City_Paper7, 
        InventoryManager.AllItems.City_Paper8 };
    private bool startingFlag = false;
    public void Picked(object sender, InventoryManager.AllItems e)
    {
        if (checkPaper.Contains(e)) papers++;
    }
    private void CheckEvents()
    {
        if(papers == 1 && !startingFlag)
        {
            Debug.Log("Triggered");
            startingFlag = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Item.PickedUp += Picked;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEvents();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City_PageCounter : MonoBehaviour
{
    private Text body;
    private Text[] children;
    private Animator anim;
    void Start()
    {
        if (AntonioManager.CheckPapers() > 0 && !InventoryManager.HasItem(InventoryManager.AllItems.City_Syringe)) TurnOn();
    }
    private void OnEnable()
    {
        
        anim = GetComponent<Animator>();
        InventoryManager.AddedItem += Count;
        if (AntonioManager.CheckPapers() > 0 && !InventoryManager.HasItem(InventoryManager.AllItems.City_Syringe)) TurnOn();
        children = GetComponentsInChildren<Text>();
        Count(null, InventoryManager.AllItems.City_AlleyKey);
    }
    private void OnDisable()
    {
        InventoryManager.AddedItem -= Count;
    }
    public void Count(object o, InventoryManager.AllItems e)
    {
        foreach(Text t in children)
        {
            t.text = "Count " + AntonioManager.CheckPapers() + " / " + "8";
        }
    }
    public void Activate()
    {
        TurnOn();
        anim.Play("RevealText");
        children = GetComponentsInChildren<Text>();
        Count(null, InventoryManager.AllItems.City_AlleyKey);
    }
    public void TurnOn()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }
}

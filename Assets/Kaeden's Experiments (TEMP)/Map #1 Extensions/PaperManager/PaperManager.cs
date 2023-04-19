using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PaperManager : MonoBehaviour
{
    public PlayableDirector[] Events;
    public IDictionary<string, PlayableDirector> reference = new Dictionary<string, PlayableDirector>();
    // Start is called before the first frame update
    void OnEnable()
    {
        InventoryManager.AddedItem += ExecuteEvent;
        InventoryManager.AddedItem += Paper1Enable;
        InventoryManager.AddedItem += ExecuteAllPages;
    }
    private void OnDisable()
    {
        InventoryManager.AddedItem -= ExecuteEvent;
        InventoryManager.AddedItem -= Paper1Enable;
        InventoryManager.AddedItem -= ExecuteAllPages;
    }
    void Awake()
    {
        Populate();
    }
    private void Populate() 
    {
        foreach (PlayableDirector pd in Events) reference.Add(pd.gameObject.name, pd);
    }
    private void Play(string playing)
    {
        if (!reference.ContainsKey(playing) || (reference[playing] == null) || !reference[playing].gameObject.activeSelf) return;
        reference[playing].Play();
    }
    public void ExecuteAllPages(object sender, InventoryManager.AllItems e)
    {
        if (!AntonioManager.IsPage(e)) return;
        if (AntonioManager.CheckPapers() >= 8) Debug.Log("collected all pages");
    }
    public void Paper1Enable(object sender, InventoryManager.AllItems e)
    {
        if (e != InventoryManager.AllItems.City_Paper1) return;
        GameObject EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore != null && AntonioManager.CheckPapers() > 0)
        {
            foreach (Transform child in EnemyStore.transform)
                child.gameObject.SetActive(true);
        }
    }
    public void ExecuteEvent(object sender, InventoryManager.AllItems e)
    {
        string play = "";
        if (e == InventoryManager.AllItems.City_Paper4) play = "Page4";
        else if (e == InventoryManager.AllItems.City_Paper3) play = "Page3";
        else if (e == InventoryManager.AllItems.City_Paper1) play = "Page1";
        else if (e == InventoryManager.AllItems.City_Paper2) play = "Page2";
        else if (e == InventoryManager.AllItems.City_Paper5) play = "Page5";
        else if (e == InventoryManager.AllItems.City_Paper6) play = "Page6";
        else if (e == InventoryManager.AllItems.City_Paper7) play = "Page7";
        else if (e == InventoryManager.AllItems.City_Paper8) play = "Page8";
        Play(play);
    }
    
}

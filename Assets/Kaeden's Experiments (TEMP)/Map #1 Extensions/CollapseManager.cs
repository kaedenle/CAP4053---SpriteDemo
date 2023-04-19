using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollapseManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static IDictionary<string, IDictionary<int, bool>> collapseTracker = new Dictionary<string, IDictionary<int, bool>>();
    private static bool DeathFlag;
    public static void SaveCollapse()
    {
        if (DeathFlag) return;
        if (!collapseTracker.ContainsKey(SceneManager.GetActiveScene().name)) collapseTracker.Add(SceneManager.GetActiveScene().name, new Dictionary<int, bool>());
        GameObject CollapseStore = GameObject.Find("-- Collapse --");
        if (CollapseStore == null) return;

        collapseTracker[SceneManager.GetActiveScene().name].Clear();
        CollapsableEnvironment[] array = CollapseStore.GetComponentsInChildren<CollapsableEnvironment>();
        foreach(CollapsableEnvironment c in array)
        {
            if(!collapseTracker[SceneManager.GetActiveScene().name].ContainsKey(c.ID)) collapseTracker[SceneManager.GetActiveScene().name].Add(c.ID, c.collapsed);
        }

    }
    public static void ReloadCollapse()
    {
        if (!collapseTracker.ContainsKey(SceneManager.GetActiveScene().name)) return;
        GameObject CollapseStore = GameObject.Find("-- Collapse --");
        if (CollapseStore == null) return;

        CollapsableEnvironment[] array = CollapseStore.GetComponentsInChildren<CollapsableEnvironment>();
        //scroll thorugh all collapsables
        foreach (CollapsableEnvironment c in array)
        {
            foreach (int ID in collapseTracker[SceneManager.GetActiveScene().name].Keys)
            {
                if(ID == c.ID && collapseTracker[SceneManager.GetActiveScene().name][c.ID])
                {
                    c.Knock();
                    break;
                }
            }
        }
    }
    public void Clear(object sender, System.EventArgs e)
    {
        foreach(string s in collapseTracker.Keys) collapseTracker[s].Clear();
        collapseTracker.Clear();
        DeathFlag = true;
    }
    private void OnEnable()
    {
        EntityManager.PlayerDead += Clear;
        DeathFlag = false;
    }
    private void OnDisable()
    {
        EntityManager.PlayerDead -= Clear;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetricsManager : MonoBehaviour
{
    private static PlayerMetricsManager instance; 
    private static IDictionary<string, int> MetricsKeeperInt = new Dictionary<string, int>();
    private static IDictionary<string, float> MetricsKeeperFloat = new Dictionary<string, float>();
    public static PlayerMetricsManager GetManager()
    {
        return instance;
    }
    public static void CreateManager()
    {
        Instantiate(Resources.Load("Prefabs/PlayerMetricsManager"));
    }
    void Awake()
    {
        //setup as a singleton
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        if(instance != this)
            Destroy(gameObject);
    }
    public static void IncrementKeeperInt(string metric)
    {
        if (!MetricsKeeperInt.ContainsKey(metric))
            MetricsKeeperInt.Add(metric, 0);
        MetricsKeeperInt[metric]++;
    }
    public static void ReturnKeeperInt()
    {
        Debug.Log("==========================");
        foreach (var s in MetricsKeeperInt.Keys)
            Debug.Log(s + ": " + MetricsKeeperInt[s]);
        Debug.Log("==========================");
    }
    public static int GetMetricInt(string metric)
    {
        if (!MetricsKeeperInt.ContainsKey(metric)) return 0;
        return MetricsKeeperInt[metric];
    }
    public static float GetMetricFloat(string metric)
    {
        if (!MetricsKeeperFloat.ContainsKey(metric)) return 0;
        return MetricsKeeperFloat[metric];
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ReturnKeeperInt();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetricsManager : MonoBehaviour
{
    private static PlayerMetricsManager instance;
    private IDictionary<string, int> MetricsKeeperInt = new Dictionary<string, int>();
    private IDictionary<string, float> MetricsKeeperFloat = new Dictionary<string, float>();
    public static PlayerMetricsManager GetManager()
    {
        if (instance == null)
            instance = (Instantiate(Resources.Load("Prefabs/PlayerMetricsManager")) as GameObject).GetComponent<PlayerMetricsManager>();
        return instance;
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
    public void IncrementKeeperInt(string metric)
    {
        if (!MetricsKeeperInt.ContainsKey(metric))
            MetricsKeeperInt.Add(metric, 0);
        MetricsKeeperInt[metric]++;
    }
    public void ReturnKeeperInt()
    {
        Debug.Log("==========================");
        foreach (var s in MetricsKeeperInt.Keys)
            Debug.Log(s + ": " + MetricsKeeperInt[s]);
        Debug.Log("==========================");
    }
    public int GetMetricInt(string metric)
    {
        if (!MetricsKeeperInt.ContainsKey(metric)) return 0;
        return MetricsKeeperInt[metric];
    }
    public float GetMetricFloat(string metric)
    {
        if (!MetricsKeeperFloat.ContainsKey(metric)) return 0;
        return MetricsKeeperFloat[metric];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

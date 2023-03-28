using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStore
{
    public Vector3 PosStore;
    public bool ResetPos;
    public int Health;
    public GameObject entity;
    public GameObject entityType;
    private HealthTracker ht;

    public EnemyStore(GameObject entity, GameObject entityType,  Vector3 pos, bool ResetPos, int Health)
    {
        this.entity = entity;
        this.entityType = entityType;
        PosStore = pos;
        this.ResetPos = ResetPos;
        this.Health = Health;
        this.ht = entity?.GetComponent<HealthTracker>();
    }
    public void HealthTrackerFinder()
    {
        this.ht = entity?.GetComponent<HealthTracker>();
    }
    public void UpdateValues()
    {
        if (!ResetPos)
            PosStore = entity.transform.position;
        if(ht != null && ht.healthSystem != null)  this.Health = ht.healthSystem.getHealth();
    }
    public void SetValues(GameObject entity)
    {
        this.entity = entity;
        HealthTrackerFinder();
        ht.SetHealth(Health);
        entity.transform.position = PosStore;
    }
}

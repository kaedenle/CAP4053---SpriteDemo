using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Base
{
    public Vector3 pos, locScale;
    public int health;
    public int spawner_index_type;

    public GameObject entity;

    public Base(GameObject entity, int spawner_index_type)
    {
        this.entity = entity;
        this.pos = entity.transform.position;
        this.locScale = entity.transform.localScale;
        // PosStore = pos;
        // this.ResetPos = ResetPos;
        this.health = entity.GetComponent<HealthTracker>().health;

        this.spawner_index_type = spawner_index_type;
    }

    public Base(Vector3 pos, Vector3 localScale, int health, int index_type)
    {
        this.pos = pos;
        this.locScale= localScale;
        this.health = health;
        this.spawner_index_type = index_type;
    }

    public void SetValues(GameObject entity)
    {
        // this.entity = entity;
        this.entity = entity;
        entity.GetComponent<HealthTracker>().SetHealth(health);
        entity.transform.localScale = locScale;
    }

    public void UpdateValues(bool recordPosition=true, bool recordHealth=true)
    {
        if(entity == null)
        {
            Debug.LogWarning("entity did not exist to update base values");
            return;
        }

        if(recordPosition)
        {
            pos = entity.transform.position;
            locScale = entity.transform.localScale;
        }

        if(recordHealth)
            health = entity.GetComponent<HealthTracker>().healthSystem.getHealth();
    }
}
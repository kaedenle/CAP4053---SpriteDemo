using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Base
{
    public VectorsAreDumb pos, locScale;
    public int health;
    public int spawner_index_type;

    public GameObject entity;

    public Base(GameObject entity, int spawner_index_type)
    {
        this.entity = entity;
        this.pos = new VectorsAreDumb(entity.transform.position);
        this.locScale = new VectorsAreDumb(entity.transform.localScale);
        this.health = entity.GetComponent<HealthTracker>().health;

        this.spawner_index_type = spawner_index_type;
    }

    public Base(Vector3 pos, Vector3 localScale, int health, int index_type)
    {
        this.pos =  new VectorsAreDumb(pos);
        this.locScale=  new VectorsAreDumb(localScale);
        this.health = health;
        this.spawner_index_type = index_type;
    }

    public void SetValues(GameObject entity)
    {
        // this.entity = entity;
        this.entity = entity;
        entity.GetComponent<HealthTracker>().SetHealth(health);
        entity.transform.localScale = locScale.Get();
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
            pos =  new VectorsAreDumb(entity.transform.position);
            locScale =  new VectorsAreDumb(entity.transform.localScale);
        }

        if(recordHealth)
            health = entity.GetComponent<HealthTracker>().healthSystem.getHealth();
    }
}
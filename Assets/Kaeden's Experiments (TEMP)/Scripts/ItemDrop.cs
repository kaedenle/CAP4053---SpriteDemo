using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemClass[] items;
    private GameObject player;
    private HealthTracker PlayerHealthTracker;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null) PlayerHealthTracker = player.GetComponent<HealthTracker>();
    }
    public void AttemptDrop()
    {
        foreach(ItemClass item in items)
        {
            float chance = 1 - item.percentChance/100 >= 0 ? 1 - item.percentChance/100 : 1;
            if (Random.value >= chance) 
                DropItem(item.item);
        } 
    }
    private void DropItem(GameObject item)
    {
        GameObject newItem = Instantiate(item, transform.position, Quaternion.identity);
        Rigidbody2D body = newItem?.GetComponent<Rigidbody2D>();
        float speed = 3;
        if (body!=null) body.velocity = -Random.onUnitSphere * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

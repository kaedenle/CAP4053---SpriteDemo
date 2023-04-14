using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDynamicSort : MonoBehaviour
{
    [SerializeField]
    public GameObject Main;
    private Renderer MainRend;
    private IDictionary<string, Renderer> rends;
    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;
        Renderer rend = obj?.GetComponent<Renderer>();
        if (rend && obj != Main && obj.name != "Player") rends.Add(obj.name, rend);
        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            GetChildRecursive(child.gameObject);
        }
    }
    private void UpdateLayers()
    {
        int projlayer = (int)(MainRend.transform.position.y * -100);
        foreach(Renderer r in rends.Values)
        {
            int delta = r.sortingOrder - MainRend.sortingOrder;
            r.sortingOrder = projlayer + delta;
        }
        MainRend.sortingOrder = projlayer;
    }
    public void CalibrateLeftArm()
    {
        rends["Left Arm"].sortingOrder = Main.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        rends = new Dictionary<string, Renderer>();
        MainRend = Main.GetComponent<Renderer>();
        GetChildRecursive(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLayers();
    }
}

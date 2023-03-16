using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawning : MonoBehaviour
{
    public GameObject spawning;
    public GameObject InfoText;
    public int LIMIT;
    private List<GameObject> AliveList;
    private float OffsetMagnitude = 5;
    private Vector3[] OffsetList = {new Vector3(-0.75f, 0.5f, 0), new Vector3(0, 1, 0) , new Vector3(0.75f, 0.5f, 0) , new Vector3(0.5f, -0.5f, 0) , new Vector3(-0.5f, -0.5f, 0)};
    public void SetAmount(int amount)
    {
        if (AliveList.Count > LIMIT && amount > 0)
        {
            Debug.Log("Too many Slime Enemies");
            return;
        }
        else if (AliveList.Count == 0 && amount < 0)
        {
            return;
        }

        if (amount > 0)
        {
            GameObject spawned = Instantiate(spawning, OffsetList[AliveList.Count] * OffsetMagnitude, Quaternion.identity);
            AliveList.Add(spawned);
        }
        if (amount < 0)
        {
            Destroy(AliveList[AliveList.Count - 1].GetComponent<HealthTracker>().bar);
            Destroy(AliveList[AliveList.Count - 1]);
            AliveList.Remove(AliveList[AliveList.Count - 1]);
        }
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        if (InfoText == null)
            return;
        if (InfoText.activeSelf)
        {
            //if text exists in object, set it, else set the children (if any) to text
            Text txt = InfoText?.GetComponent<Text>();
            //set body and shadow to text
            if (txt == null)
            {
                foreach (Transform child in InfoText.transform)
                    child.gameObject.GetComponent<Text>().text = AliveList.Count.ToString();
            }
            else
            {
                txt.text = AliveList.Count.ToString();
            }
        }
    }
    private void AutoUpdateList()
    {
        foreach(GameObject go in AliveList)
        {
            if (go == null || go.name == "null")
            {
                AliveList.Remove(go);
            }  
        }
        UpdateGUI();
    }
    // Start is called before the first frame update
    void Awake()
    {
        AliveList = new List<GameObject>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        AutoUpdateList();   
    }
}

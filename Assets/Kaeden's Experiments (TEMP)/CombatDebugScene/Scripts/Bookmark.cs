using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookmark : MonoBehaviour
{
    public int PageID;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        RectTransform child = transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform myTrans = GetComponent<RectTransform>();
        if (PageID == BookUI.PageIndex)
        {
            myTrans.sizeDelta = new Vector2(9, myTrans.sizeDelta.y);
            myTrans.anchoredPosition = new Vector2(0.39f, myTrans.anchoredPosition.y);
        }
        else
        {
            myTrans.sizeDelta = new Vector2(6, myTrans.sizeDelta.y);
            myTrans.anchoredPosition = new Vector2(1.58f, myTrans.anchoredPosition.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

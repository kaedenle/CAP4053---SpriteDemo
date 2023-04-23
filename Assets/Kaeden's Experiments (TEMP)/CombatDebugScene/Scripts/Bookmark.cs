using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookmark : MonoBehaviour
{
    public int PageID;
    private const float POSPAGE = 0.55f;
    private const float POSPAGEREV = -89.51f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        RectTransform child = transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform myTrans = GetComponent<RectTransform>();
        float use = Mathf.Abs(myTrans.localScale.x);
        if (PageID == BookUI.PageIndex)
        {
            myTrans.sizeDelta = new Vector2(9, myTrans.sizeDelta.y);
            myTrans.anchoredPosition = new Vector2(0.39f, myTrans.anchoredPosition.y);
            myTrans.localScale = new Vector2(use, myTrans.localScale.y);
        }
        else if(PageID > BookUI.PageIndex)
        {
            myTrans.sizeDelta = new Vector2(6, myTrans.sizeDelta.y);
            myTrans.anchoredPosition = new Vector2(POSPAGE + XPosModifier(), myTrans.anchoredPosition.y);

            myTrans.localScale = new Vector2(use, myTrans.localScale.y);
        }
        else if(PageID < BookUI.PageIndex)
        {
            myTrans.sizeDelta = new Vector2(6, myTrans.sizeDelta.y);
            myTrans.anchoredPosition = new Vector2(POSPAGEREV - XPosModifier(), myTrans.anchoredPosition.y);
            myTrans.localScale = new Vector2(-use, myTrans.localScale.y);
        }
    }
    private float XPosModifier()
    {
        return (Mathf.Abs(PageID - BookUI.PageIndex) - 1) * 0.25f;
    }
    // Update is called once per frame
    void LateUpdate()
    {
    }
}

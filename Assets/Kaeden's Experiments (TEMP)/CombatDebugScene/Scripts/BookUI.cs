using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{
    public bool IsOut;
    private Animator anim;
    private SpriteRenderer sr;
    private AudioSource audiosrc;
    public GameObject[] pages;
    private bool DoneFlag = false;
    private GameObject RightPage;
    private GameObject LeftPage;
    private int PageIndex;
    private bool RememberPages = true;
    public void ToggleBook()
    {
        IsOut = EntityManager.IsPaused();
        if (!IsOut)
        {
            anim.ResetTrigger("In");
            anim.Play("Buffer");
            DoneFlag = false;
            if (!RememberPages) PageIndex = 0;
            TurnOffChildren();
        }
        gameObject.SetActive(IsOut);
        //gameObject.transform.GetChild(0).gameObject.SetActive(IsOut);
        if (IsOut) anim.SetTrigger("In");
    }
    public void SkipAnim()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        audiosrc.Stop();
        string animName = "Buffer";
        if(clipinfo.Length > 0) animName = clipinfo[0].clip.name;

        if (IsOut && animName != "Buffer")
            anim.CrossFade("Pop In", 0f, 0, 0.94f);
    }
    private void TurnOffChildren()
    {
        //turn off all children by default
        foreach (Transform child in gameObject.transform)
            child.gameObject.SetActive(false);
    }
    public void Done()
    {
        DoneFlag = true;
        LoadPages();
    }
    private void ChangeText(string text, GameObject textObject)
    {
        GameObject child = textObject.transform.GetChild(0).gameObject;
        GameObject grandchild = child.transform.GetChild(0).gameObject;

        Text txt = textObject?.GetComponent<Text>();
        if (txt != null) txt.text = text;
        Text ctxt = child?.GetComponent<Text>();
        if (ctxt != null) ctxt.text = text;
        Text gctxt = grandchild?.GetComponent<Text>();
        if (gctxt != null) gctxt.text = text;

    }
    public void PageFlip(int jump)
    {
        if (pages.Length == 0 || !DoneFlag) return;
        //if pages out of range
        if (PageIndex + jump > ((pages.Length - 1) / 2) || PageIndex + jump < 0) return;

        if(LeftPage != null) LeftPage.SetActive(false);
        LeftPage = null;

        if(RightPage != null) RightPage.SetActive(false);
        RightPage = null;
        if (jump > 0)
            anim.Play("Flip Page 1");
        else
            anim.Play("Flip Page 2");
        PageIndex += jump;
    }
    public void LoadPages()
    {
        if (pages.Length == 0) return;
        LeftPage = pages[PageIndex * 2];
        LeftPage.SetActive(true);

        if(PageIndex * 2 + 1 < pages.Length)
        {
            RightPage = pages[PageIndex * 2 + 1];
            RightPage.SetActive(true);
        }

        ChangeText((PageIndex * 2 + 1).ToString(), gameObject.transform.Find("PageNumberLeft").gameObject);
        ChangeText((PageIndex * 2 + 2).ToString(), gameObject.transform.Find("PageNumberRight").gameObject);
    }
    private void input()
    {
        //skip book animation
        if (Input.GetKeyDown(KeyCode.Mouse0) && !DoneFlag)
            SkipAnim();
        //flip page forward
        if (Input.GetKeyDown(KeyCode.A) && DoneFlag)
            PageFlip(-1);
        //flip page backwards
        if (Input.GetKeyDown(KeyCode.D) && DoneFlag)
            PageFlip(1);
    }
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audiosrc = GetComponent<AudioSource>();
        TurnOffChildren();
        
    }

    // Update is called once per frame
    void Update()
    {
        input();
    }
}

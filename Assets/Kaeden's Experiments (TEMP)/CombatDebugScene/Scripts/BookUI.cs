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
    public static int PageIndex;
    private bool RememberPages = true;

    public GameObject RightArrow;
    public GameObject LeftArrow;
    private Button RightButton;
    private Button LeftButton;
    private int GoToPage = -1;
    private bool TurningPage = false;
    public void ToggleBook()
    {
        IsOut = UIManager.IsPaused();
        if (!IsOut)
        {
            anim.ResetTrigger("In");
            anim.Play("Buffer");
            DoneFlag = false;
            if (!RememberPages) PageIndex = 0;
            TurnOffChildren();
        }
        gameObject.SetActive(IsOut);
        GoToPage = -1;
        TurningPage = false;
        //gameObject.transform.GetChild(0).gameObject.SetActive(IsOut);
        if (IsOut) anim.SetTrigger("In");
    }
    private void CheckArrows()
    {
        DoneFlag = true;
        if (PageIndex == 0 && LeftButton != null)
            LeftButton.interactable = false;
        else
            LeftButton.interactable = true;

        if (PageIndex == ((pages.Length - 1) / 2) && RightButton != null)
            RightButton.interactable = false;
        else
            RightButton.interactable = true;
    }
    public void SkipAnim()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        audiosrc.Stop();
        string animName = "Buffer";
        if (clipinfo.Length > 0) animName = clipinfo[0].clip.name;

        if (IsOut && animName != "Buffer")
            anim.CrossFade("Pop In", 0f, 0, 0.94f);
        //GoToPage = -1;
    }
    public void SkipAnimPage()
    {
        audiosrc.Stop();
        PageIndex = GoToPage;
        DoneFlag = true;
        TurningPage = false;
        GoToPage = -1;
        LoadPages();
        anim.Play("Buffer");
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
    public void SetFlip(int number)
    {
        if (number == PageIndex) return;
        GoToPage = number;
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
        if (PageIndex + jump > ((pages.Length - 1) / 2) || PageIndex + jump < 0)
        {   
            return;
        }
        TurningPage = true;
        if(LeftPage != null) LeftPage.SetActive(false);
        LeftPage = null;

        if(RightPage != null) RightPage.SetActive(false);
        RightPage = null;
        if (jump > 0)
            anim.Play("Flip Page 1");
        else
            anim.Play("Flip Page 2");
        PageIndex += jump;
        CheckArrows();
        
    }
    public void LoadPages()
    {
        if (pages.Length == 0) return;
        LeftPage = pages[PageIndex * 2];
        LeftPage.SetActive(true);
        if (PageIndex * 2 + 1 < pages.Length)
        {
            RightPage = pages[PageIndex * 2 + 1];
            RightPage.SetActive(true);
        }
        if(LeftArrow != null) LeftArrow.SetActive(true);
        if (RightArrow != null) RightArrow.SetActive(true);
        ChangeText((PageIndex * 2 + 1).ToString(), gameObject.transform.Find("PageNumberLeft").gameObject);
        ChangeText((PageIndex * 2 + 2).ToString(), gameObject.transform.Find("PageNumberRight").gameObject);
    }
    public void FinishTurn()
    {
        TurningPage = true;
    }
    private void CheckBookmark()
    {
        if (GoToPage == -1 || GoToPage - PageIndex == 0)
        {
            TurningPage = false;
            GoToPage = -1;
            return;
        }
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipinfo.Length > 0 && clipinfo[0].clip.name != "Buffer") return;
        int turn = 1;
        if (GoToPage - PageIndex < 0)
            turn = -1;
        PageFlip(turn);
    }
    private void input()
    {
        //skip book animation
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!DoneFlag) SkipAnim();
            else if (TurningPage) SkipAnimPage();
        }
            
        //flip page forward
        if (Input.GetKeyDown(KeyCode.A) && !TurningPage)
            PageFlip(-1);
        //flip page backwards
        if (Input.GetKeyDown(KeyCode.D) && !TurningPage)
            PageFlip(1);
    }
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audiosrc = GetComponent<AudioSource>();
        if(LeftArrow != null) LeftButton = LeftArrow?.GetComponentInChildren<Button>();
        if(RightArrow != null) RightButton = RightArrow?.GetComponentInChildren<Button>();
        TurnOffChildren();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckBookmark();
        input();

    }
}

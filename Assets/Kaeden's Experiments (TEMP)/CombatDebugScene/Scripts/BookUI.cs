using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookUI : MonoBehaviour
{
    public bool IsOut;
    private Animator anim;
    private SpriteRenderer sr;
    public void ToggleBook()
    {
        IsOut = EntityManager.IsPaused();
        if (!IsOut)
        {
            anim.ResetTrigger("In");
            anim.Play("Buffer");
        }
        gameObject.SetActive(IsOut);
        //gameObject.transform.GetChild(0).gameObject.SetActive(IsOut);
        if (IsOut) anim.SetTrigger("In");
    }

    public void msg()
    {
        Debug.Log("hi");
    }
    public void SkipAnim()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        string animName = "Buffer";
        if(clipinfo.Length > 0) animName = clipinfo[0].clip.name;

        if (IsOut && animName != "Buffer")
            anim.CrossFade("Pop In", 0f, 0, 0.94f);

    }

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

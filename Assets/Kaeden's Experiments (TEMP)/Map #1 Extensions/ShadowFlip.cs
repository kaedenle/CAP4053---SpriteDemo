using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFlip : MonoBehaviour
{
    private SpriteRenderer mySR;
    private SpriteRenderer parentSR;
    // Start is called before the first frame update
    void Start()
    {
        mySR = GetComponent<SpriteRenderer>();
        parentSR = gameObject.transform.parent.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (mySR.flipX != parentSR.flipX) mySR.flipX = parentSR.flipX;
    }
}

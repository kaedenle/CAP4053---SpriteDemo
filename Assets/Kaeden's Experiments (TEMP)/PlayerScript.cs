using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        bool flipped = animator.GetBool("flipped");
        int temp = flipped ? 180 : 0;

        //update the Weapon tag to rotate correctly (all will be animated the same)
        GameObject[] wpnObjs = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject wpn in wpnObjs)
        {
            GameObject parent = wpn.transform.parent.gameObject;
            //flip and flip to default
            if (wpn.transform.rotation.eulerAngles.y < 180 && flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 180);
            else if (wpn.transform.rotation.eulerAngles.y >= 180 && !flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 0);
        }
    }
}

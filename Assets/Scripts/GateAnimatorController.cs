using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAnimatorController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("foundKey", InventoryManager.PickedUp(InventoryManager.AllItems.MobsterKeyDemo));
    }
}

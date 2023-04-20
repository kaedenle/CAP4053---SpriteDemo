using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChildDiary : MonoBehaviour
{
    public GameObject lightOn;
    public Animator bookOpen;
    public Animator bookClose;
    public GameObject DiaryUI;

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
            DiaryUI.SetActive(true);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) 
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         DiaryUI.SetActive(true);
    //     }
    // }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            DiaryUI.SetActive(false);
        }
    }

    private void Update() 
    {
        // if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && noteImage.enabled)
        // {
        //     noteImage.enabled = false;
        //     text1.enabled = false;
        //     text2.enabled = false;
        //     text3.enabled = false;
        //     bookClose.Play("B");
        // }
    }

}

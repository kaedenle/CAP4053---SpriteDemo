using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChildDiary : MonoBehaviour
{
    [SerializeField] private Image noteImage;
    [SerializeField] private TMP_Text text1;
    [SerializeField] private TMP_Text text2;
    [SerializeField] private TMP_Text text3;
    public GameObject lightOn;
    public Animator bookOpen;
    public Animator bookClose;

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            noteImage.enabled = true;
            text1.enabled = true;
            text2.enabled = true;
            text3.enabled = true;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) 
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         noteImage.enabled = true;
    //         text1.enabled = true;
    //         text2.enabled = true;
    //         text3.enabled = true;
    //     }
    // }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            noteImage.enabled = false;
            text1.enabled = false;
            text2.enabled = false;
            text3.enabled = false;
        }
    }

    private void Update() 
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && noteImage.enabled)
        {
            noteImage.enabled = true;
            text1.enabled = true;
            text2.enabled = true;
            text3.enabled = true;
        }
    }

}

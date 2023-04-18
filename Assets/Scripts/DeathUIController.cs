using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
    private Animator animator;
    private string trigger = "trigger";
    private bool triggered = false;
    public bool IsDebug;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(!triggered && LevelManager.PlayerDead())
        {
            animator.SetTrigger(trigger);
            triggered = true;
        }
    }
    void OnEnable()
    {
        Cursor.visible = true;
    }
    
    void OnDisable()
    {
        Cursor.visible = false;
    }

    public void ResetButton()
    {
        //ONLY FOR KAEDEN'S DEBUG MODE
        if (IsDebug)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LevelManager.ResetVariables();
            return;
        }
           
        LevelManager.CheckpointButton();
    }
}

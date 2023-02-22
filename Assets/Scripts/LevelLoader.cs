using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Animator animator;
    private string trigger = "play";
    private float transitionTime = 1f;



    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        Debug.Log("grabbed animator");
    }

    public static string Test()
    {
        return "reached here";
    }

    public IEnumerator Fade(int idx)
    {
        // Play animation
        Debug.Log("triggering animator...");
        animator.SetTrigger(trigger);
        Debug.Log("about to wait");

        // wait
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(idx);

        //load scene
        Debug.Log("waited for 1 s");
    }
}

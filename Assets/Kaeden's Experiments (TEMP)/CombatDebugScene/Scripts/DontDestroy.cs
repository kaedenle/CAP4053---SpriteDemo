using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private bool Active = false;
    private static GameObject instance;
    private Animator anim;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    public bool isActive()
    {
        return Active;
    }
    public void FadeIn()
    {
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anim.Play("Black Fade In");
        Active = true;
    }

    public void FadeOut()
    {
        anim.Play("Black Fade Out");
        Active = false;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LevelManager.ResetVariables();
        anim.updateMode = AnimatorUpdateMode.Normal;
    }
    public void DeactivateSelf()
    {
        anim.SetBool("Play", false);
        enabled = false;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {

    }
}

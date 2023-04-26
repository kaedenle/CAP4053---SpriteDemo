using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    public GameObject warning;
    public bool delayDone = false, triggered = false;

    void Awake()
    {
        if(warning != null) warning.SetActive(false); // start off by default
    }
   
    void Update()
    {
        if(!triggered && InputManager.AnyKeyPressed())
        {
            if(warning == null && InputManager.ContinueKeyPressed())    // case out if warning instruction is null
                Activate();
            else if(warning != null)
            {
                StartCoroutine(ActivateConfirmMessage()); // REMOVE COROUTINE if animation is added *****
                StartCoroutine(DelayInput());
                triggered = true;
            }
        }

        if(delayDone && InputManager.ContinueKeyPressed())
        {
            Activate();
        }
    }

    IEnumerator ActivateConfirmMessage()
    {
        // PUT ANIMATION TRIGGER HERE, if applicable *****

        yield return new WaitForSeconds(1.0F); // delete this if an animation is added *****
        warning.SetActive(true);
    }

    IEnumerator DelayInput()
    {
        yield return new WaitForSeconds(1.0F);
        delayDone = true;
    }

    void Activate()
    {
        if(ScenesManager.GetCurrentScene() == ScenesManager.AllScenes.StartCutScene)
        {
            SceneManager.LoadScene("Central Hub");
        }

        if(ScenesManager.GetCurrentScene() == ScenesManager.AllScenes.Credits)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}

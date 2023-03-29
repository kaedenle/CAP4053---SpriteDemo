using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    //hitstop variables
    private bool waiting = false;
    private bool ResumeTime = false;
    //Camera variables
    private Camera MainCam;
    public void ShakeCam(float duration, float magnitude)
    {
        CameraShake cs = MainCam.GetComponent<CameraShake>();
        StopCoroutine(cs.Shake(duration, magnitude));
        StartCoroutine(cs.Shake(duration, magnitude));
    }
    public void StopTime(float duration)
    {
        if (waiting || Time.timeScale != 1)
            return;
        //change this if want to go slower rather than stop
        Time.timeScale = 0;
        StopCoroutine(Wait(duration));
        StartCoroutine(Wait(duration));
    }
    IEnumerator Wait(float amt)
    {
        waiting = true;

        yield return new WaitForSecondsRealtime(amt);
        //ResumeTime = true;
        if(!EntityManager.IsPaused()) Time.timeScale = 1f;
        waiting = false;
    }

    private void Update()
    {
        if (ResumeTime)
        {
            /*if (Time.timeScale < 1f)
            {
                float time = Time.deltaTime == 0 ? 0.1f : Time.deltaTime * 3;
                Time.timeScale += time;
            }
            else
            {
                Time.timeScale = 1f;
                ResumeTime = false;
            }*/
            Time.timeScale = 1f;
            ResumeTime = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //add pausing logic here
        Time.timeScale = 1f;
        MainCam = Camera.main;
    }
}

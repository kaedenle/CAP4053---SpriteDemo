using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class CutsceneLoader : MonoBehaviour
{
    // animator variables for the cut scene
    private Animator animator;
    private string trigger = "start";
    private bool on = false, delayComplete = false;
    private float delay = 1.5F;

    // text file of the cut scene
    public TextAsset cutSceneTextFile;
    // the next scene
    [SerializeField] public ScenesManager.AllScenes nextScene;

    // keep track of the text of the cut scene
    private string cutSceneText;
    public TMP_Text wordsTextField;
    public bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        cutSceneText = getText(cutSceneTextFile);
    }

    // Update is called once per frame
    void Update()
    {
        if(!on && LevelManager.IsEndOfLevel())
        {
            wordsTextField.text = cutSceneText;
            animator.SetTrigger(trigger);
            on = true;
            StartCoroutine(ControlDelay());
        }

        if(on && delayComplete && Input.anyKeyDown)
        {
            if(!isEnd)
                LevelManager.EndLevel();
            else
            {
                // CHANGE THIS ONCE AN END BEHAVIOR IS DEFINED
                GameState game = GameState.LoadGame();
                game.IncrementStateAndSave();
                ScenesManager.LoadScene(ScenesManager.AllScenes.Menu);
            }
                
            delayComplete = false;
        }
    }

    IEnumerator ControlDelay()
    {
        yield return new WaitForSecondsRealtime(delay);
        delayComplete = true;
    }

    // returns the text of the input file asset
    string getText(TextAsset input)
    {
        return input.text;
    }
}


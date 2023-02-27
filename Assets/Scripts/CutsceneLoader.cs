using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEditor;

public class CutsceneLoader : MonoBehaviour
{
    private Animator animator;
    private string trigger = "start";
    private bool on = false;

    [SerializeField] public ScenesManager.AllScenes nextScene;
    public bool demoForks = false;
    [SerializeField] public ScenesManager.AllScenes nextDemoScene;

    private string cutSceneText = "[placeholder text: I looked out across the vast scape of death before me.  Only a little girl remained, crying beside her mother and father, pleading for them to wake up.]";
    private string demoCutSceneText = "Thank you for playing our demo!";

    public TMP_Text wordsTextField;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!on && MobsterLevelManager.IsEndOfLevel())
        {
            wordsTextField.text = ((demoForks && ScenesManager.isDemo()) ? demoCutSceneText : cutSceneText);
            Debug.Log("set wordsTextField.text to " + wordsTextField.text);
            animator.SetTrigger(trigger);
            on = true;
        }

        if(on && Input.anyKeyDown)
        {
            if (demoForks && ScenesManager.isDemo()) ScenesManager.LoadScene(nextDemoScene);
            else ScenesManager.LoadScene(nextScene);
        }
    }
}


// custom editor
[CustomEditor(typeof(CutsceneLoader))]
public class MyScriptEditor2 : Editor
{
    #region
    SerializedProperty wordsTextField;
    SerializedProperty nextScene;
    //SerializedProperty cutSceneText;
    SerializedProperty demoForks;
    SerializedProperty nextDemoScene;
    //SerializedProperty demoCutSceneText;
    # endregion

    private void OnEnable()
    {
        wordsTextField = serializedObject.FindProperty("wordsTextField");
        nextScene = serializedObject.FindProperty("nextScene");
        //cutSceneText = serializedObject.FindProperty("cutSceneText");
        demoForks = serializedObject.FindProperty("demoForks");
        nextDemoScene = serializedObject.FindProperty("nextDemoScene");
        //demoCutSceneText = serializedObject.FindProperty("demoCutSceneText");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(wordsTextField);
        EditorGUILayout.PropertyField(nextScene);
        //EditorGUILayout.PropertyField(cutSceneText);
        EditorGUILayout.PropertyField(demoForks);

        if (demoForks.boolValue)
        {
            EditorGUILayout.PropertyField(nextDemoScene);
            //EditorGUILayout.PropertyField(demoCutSceneText);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

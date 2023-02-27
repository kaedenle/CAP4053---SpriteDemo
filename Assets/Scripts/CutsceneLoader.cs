using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEditor;

public class CutsceneLoader : MonoBehaviour
{
    // animator variables for the cut scene
    private Animator animator;
    private string trigger = "start";
    private bool on = false;

    // text file of the cut scene
    public TextAsset cutSceneTextFile;
    // the next scene
    [SerializeField] public ScenesManager.AllScenes nextScene;

    // demo versions of above (marked if the demo versions are different)
    public bool demoForks = false;
    [SerializeField] public ScenesManager.AllScenes nextDemoScene;
    public TextAsset demoCutSceneTextFile;

    // keep track of the text of the cut scene
    private string cutSceneText;
    private string demoCutSceneText;

    public TMP_Text wordsTextField;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        cutSceneText = getText(cutSceneTextFile);
        if(demoForks) demoCutSceneText = getText(demoCutSceneTextFile);
    }

    // Update is called once per frame
    void Update()
    {
        if(!on && LevelManager.IsEndOfLevel())
        {
            wordsTextField.text = ((demoForks && ScenesManager.isDemo()) ? demoCutSceneText : cutSceneText);
            // Debug.Log("set wordsTextField.text to " + wordsTextField.text);
            animator.SetTrigger(trigger);
            on = true;
        }

        if(on && Input.anyKeyDown)
        {
            if (demoForks && ScenesManager.isDemo()) ScenesManager.LoadScene(nextDemoScene);
            else ScenesManager.LoadScene(nextScene);
        }
    }

    // returns the text of the input file asset
    string getText(TextAsset input)
    {
        return input.text;
    }
}


// custom editor for only showing stuff if the demoForks bool is true
[CustomEditor(typeof(CutsceneLoader))]
public class MyScriptEditor2 : Editor
{
    #region
    SerializedProperty wordsTextField;
    SerializedProperty nextScene;
    SerializedProperty cutSceneTextFile;
    SerializedProperty demoForks;
    SerializedProperty nextDemoScene;
    SerializedProperty demoCutSceneTextFile;
    # endregion

    private void OnEnable()
    {
        wordsTextField = serializedObject.FindProperty("wordsTextField");
        nextScene = serializedObject.FindProperty("nextScene");
        cutSceneTextFile = serializedObject.FindProperty("cutSceneTextFile");
        demoForks = serializedObject.FindProperty("demoForks");
        nextDemoScene = serializedObject.FindProperty("nextDemoScene");
        demoCutSceneTextFile = serializedObject.FindProperty("demoCutSceneTextFile");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(wordsTextField);
        EditorGUILayout.PropertyField(nextScene);
        EditorGUILayout.PropertyField(cutSceneTextFile);
        EditorGUILayout.PropertyField(demoForks);

        if (demoForks.boolValue)
        {
            EditorGUILayout.PropertyField(nextDemoScene);
            EditorGUILayout.PropertyField(demoCutSceneTextFile);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class CutsceneLoader : MonoBehaviour
{
    private Animator animator;
    private string trigger = "start";
    private bool on = false;

    [SerializeField] public ScenesManager.AllScenes nextScene;
    public bool demoForks = false;
    [SerializeField] public ScenesManager.AllScenes nextDemoScene;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MobsterLevelManager.IsEndOfLevel())
        {
            animator.SetTrigger(trigger);
            on = true;
        }

        if(on && Input.anyKeyDown)
        {
            if (demoForks) ScenesManager.LoadSceneChoice(nextScene, nextDemoScene);
            else ScenesManager.LoadScene(nextScene);
        }
    }
}


// custom editor
[CustomEditor(typeof(CutsceneLoader))]
public class MyScriptEditor2 : Editor
{
    #region
    SerializedProperty nextScene;
    SerializedProperty nextDemoScene;
    SerializedProperty demoForks;
    # endregion

    private void OnEnable()
    {
        nextScene = serializedObject.FindProperty("nextScene");
        demoForks = serializedObject.FindProperty("demoForks");
        nextDemoScene = serializedObject.FindProperty("nextDemoScene");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(nextScene);
        EditorGUILayout.PropertyField(demoForks);

        if (demoForks.boolValue)
            EditorGUILayout.PropertyField(nextDemoScene);

        serializedObject.ApplyModifiedProperties();
    }
}

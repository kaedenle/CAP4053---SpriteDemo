using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using UnityEditor;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] public ScenesManager.AllScenes nextScene;
    public bool demoForks = false;
    [SerializeField] public ScenesManager.AllScenes nextDemoScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        // should probably throw an exception here if the scene info is not valid
        // sends the next scene or scenes to ScenesManager to load the next appropriate scene
        if (demoForks) ScenesManager.LoadSceneChoice(nextScene, nextDemoScene);
        else ScenesManager.LoadScene(nextScene);
    }

}

// custom editor will allow you to choose a demo scene only if the demo forks
[CustomEditor(typeof(SceneSwitch))]
public class MyScriptEditor : Editor
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

        if(demoForks.boolValue)
            EditorGUILayout.PropertyField(nextDemoScene);

        serializedObject.ApplyModifiedProperties();
    }
}
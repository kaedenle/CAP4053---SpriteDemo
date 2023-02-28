using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

// custom editor will allow you to choose a demo scene only if the demo forks
[CustomEditor(typeof(SceneSwitch))]
public class SceneSwitchEditor : Editor
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
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using UnityEditor;


// // custom editor for only showing stuff if the demoForks bool is true
// [CustomEditor(typeof(CutsceneLoader))]
// public class CutSceneLoaderEditor : Editor
// {
//     #region
//     SerializedProperty wordsTextField;
//     SerializedProperty nextScene;
//     SerializedProperty cutSceneTextFile;
//     SerializedProperty demoForks;
//     SerializedProperty nextDemoScene;
//     SerializedProperty demoCutSceneTextFile;
//     # endregion

//     private void OnEnable()
//     {
//         wordsTextField = serializedObject.FindProperty("wordsTextField");
//         nextScene = serializedObject.FindProperty("nextScene");
//         cutSceneTextFile = serializedObject.FindProperty("cutSceneTextFile");
//         demoForks = serializedObject.FindProperty("demoForks");
//         nextDemoScene = serializedObject.FindProperty("nextDemoScene");
//         demoCutSceneTextFile = serializedObject.FindProperty("demoCutSceneTextFile");
//     }

//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();

//         EditorGUILayout.PropertyField(wordsTextField);
//         EditorGUILayout.PropertyField(nextScene);
//         EditorGUILayout.PropertyField(cutSceneTextFile);
//         EditorGUILayout.PropertyField(demoForks);

//         if (demoForks.boolValue)
//         {
//             EditorGUILayout.PropertyField(nextDemoScene);
//             EditorGUILayout.PropertyField(demoCutSceneTextFile);
//         }

//         serializedObject.ApplyModifiedProperties();
//     }
// }

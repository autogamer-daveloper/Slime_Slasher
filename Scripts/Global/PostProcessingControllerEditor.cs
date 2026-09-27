#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PostProcessingController))]
public class PostProcessingControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty isMainMenu = serializedObject.FindProperty("isMainMenu");
        SerializedProperty inputType = serializedObject.FindProperty("inputType");

        EditorGUILayout.PropertyField(isMainMenu);
        EditorGUILayout.PropertyField(inputType);

        if(isMainMenu.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("toggle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("src"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clip"));
        }

        if(!isMainMenu.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("volumes"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
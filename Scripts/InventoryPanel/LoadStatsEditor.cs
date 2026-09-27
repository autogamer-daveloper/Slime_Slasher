#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoadStats))]
public class LoadStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("textSource"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("yourValue"));

        SerializedProperty isUsingPrefix = serializedObject.FindProperty("isUsingPrefix");
        SerializedProperty prefix = serializedObject.FindProperty("prefix");

        EditorGUILayout.PropertyField(isUsingPrefix);

        if(isUsingPrefix.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(prefix);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
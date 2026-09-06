#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HideInterface))]
public class HideInterfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty isDebug = serializedObject.FindProperty("isDebug");
        SerializedProperty ui = serializedObject.FindProperty("ui");

        EditorGUILayout.PropertyField(isDebug);

        if(isDebug.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(ui);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
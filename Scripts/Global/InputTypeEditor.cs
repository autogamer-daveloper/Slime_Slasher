#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputType))]
public class InputTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty buildType = serializedObject.FindProperty("buildType");
        SerializedProperty isHaveMobileObjects = serializedObject.FindProperty("isHaveMobileObjects");
        SerializedProperty isHaveComputerObjects = serializedObject.FindProperty("isHaveComputerObjects");

        EditorGUILayout.PropertyField(buildType);
        EditorGUILayout.PropertyField(isHaveMobileObjects);
        EditorGUILayout.PropertyField(isHaveComputerObjects);

        if(isHaveMobileObjects.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mobileObjects"));
        }

        if(isHaveComputerObjects.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("computerObjects"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
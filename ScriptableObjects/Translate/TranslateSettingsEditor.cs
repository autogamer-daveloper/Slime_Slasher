#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TranslateSettingsContainer))]
public class TranslateSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty isUsingFontAsset = serializedObject.FindProperty("isUsingFontAsset");
        SerializedProperty recalculateScale = serializedObject.FindProperty("recalculateScale");
        SerializedProperty isCensored = serializedObject.FindProperty("isCensored");

        EditorGUILayout.PropertyField(isUsingFontAsset);

        if(isUsingFontAsset.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fontAsset"));
        }

        EditorGUILayout.PropertyField(recalculateScale);

        if(recalculateScale.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scaler"));
        }

        EditorGUILayout.PropertyField(isCensored);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
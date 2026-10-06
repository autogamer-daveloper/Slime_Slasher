#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Censor))]
public class CensorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("globalSettings"));

        SerializedProperty useResprite = serializedObject.FindProperty("useResprite");
        EditorGUILayout.PropertyField(useResprite);

        if(useResprite.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("resprite"));
        }

        SerializedProperty useReimage = serializedObject.FindProperty("useReimage");
        EditorGUILayout.PropertyField(useReimage);

        if(useReimage.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("reimage"));
        }

        SerializedProperty useSpriteRecolor = serializedObject.FindProperty("useSpriteRecolor");
        EditorGUILayout.PropertyField(useSpriteRecolor);

        if(useSpriteRecolor.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spriteRecolor"));
        }

        SerializedProperty useUIRecolor = serializedObject.FindProperty("useUIRecolor");
        EditorGUILayout.PropertyField(useUIRecolor);

        if(useUIRecolor.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uiRecolor"));
        }

        SerializedProperty useParticlesRecolor = serializedObject.FindProperty("useParticlesRecolor");
        EditorGUILayout.PropertyField(useParticlesRecolor);

        if(useParticlesRecolor.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particleRecolor"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
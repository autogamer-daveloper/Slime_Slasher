#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SaveManager.MomentsBoard
{
    [CustomEditor(typeof(MomentsSaveManager))]
    public class MomentsSaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty usingInterface =
                serializedObject.FindProperty("usingInterface");

            SerializedProperty memories =
                serializedObject.FindProperty("memories");

            SerializedProperty unknownSprite =
                serializedObject.FindProperty("unknownSprite");

            SerializedProperty callAtStart =
                serializedObject.FindProperty("callAtStart");

            SerializedProperty atStart =
                serializedObject.FindProperty("atStart");

            EditorGUILayout.PropertyField(usingInterface);

            if (usingInterface.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(memories, true);
                EditorGUILayout.PropertyField(unknownSprite);
            }

            EditorGUILayout.PropertyField(callAtStart);

            if (callAtStart.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(atStart);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
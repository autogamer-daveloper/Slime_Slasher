#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SaveManager.MomentsBoard
{
    [CustomEditor(typeof(MomentsSaveManager))]
    public class MomentsSaveManagerEditor : Editor
    {
        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;

        private const float HeaderHeight = 52f;
        private const float AccentHeight = 6f;

        private void InitializeStyles()
        {
            if (titleStyle != null)
                return;

            titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleLeft
            };

            subtitleStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                fontSize = 9,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            InitializeStyles();

            serializedObject.Update();

            DrawCustomHeader();

            EditorGUILayout.Space(6f);

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

        // ─────────────────────────────────────────────────────────────
        // CUSTOM HEADER
        // ─────────────────────────────────────────────────────────────

        private void DrawCustomHeader()
        {
            Rect rect = EditorGUILayout.GetControlRect(
                false,
                HeaderHeight
            );

            Color backgroundColor = EditorGUIUtility.isProSkin
                ? new Color(0.12f, 0.12f, 0.12f)
                : new Color(0.78f, 0.78f, 0.78f);

            Color accentColor = new Color(
                0.55f,
                0.18f,
                0.85f
            );

            Color separatorColor = EditorGUIUtility.isProSkin
                ? new Color(0.28f, 0.28f, 0.28f)
                : new Color(0.55f, 0.55f, 0.55f);

            Color subtitleColor = EditorGUIUtility.isProSkin
                ? new Color(0.65f, 0.65f, 0.65f)
                : new Color(0.35f, 0.35f, 0.35f);

            // Background
            EditorGUI.DrawRect(
                rect,
                backgroundColor
            );

            // Accent bar
            EditorGUI.DrawRect(
                new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    AccentHeight
                ),
                accentColor
            );

            // Title
            titleStyle.normal.textColor = Color.white;

            GUI.Label(
                new Rect(
                    rect.x + 14f,
                    rect.y + 10f,
                    rect.width - 100f,
                    22f
                ),
                "Moments save manager",
                titleStyle
            );

            // Subtitle
            subtitleStyle.normal.textColor = subtitleColor;

            GUI.Label(
                new Rect(
                    rect.x + 14f,
                    rect.y + 30f,
                    rect.width - 100f,
                    16f
                ),
                "A Games Studio",
                subtitleStyle
            );

            // Bottom separator
            EditorGUI.DrawRect(
                new Rect(
                    rect.x,
                    rect.yMax - 1f,
                    rect.width,
                    1f
                ),
                separatorColor
            );
        }
    }
}

#endif
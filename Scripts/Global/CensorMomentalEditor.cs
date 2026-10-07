#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CensorMomental))]
public class CensorMomentalEditor : Editor
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

        SerializedProperty useHidingObjects = serializedObject.FindProperty("useHidingObjects");
        EditorGUILayout.PropertyField(useHidingObjects);

        if(useHidingObjects.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objects"));
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
            "CENSOR",
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

#endif
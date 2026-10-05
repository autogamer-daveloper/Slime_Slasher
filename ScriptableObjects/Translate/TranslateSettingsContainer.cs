using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TranslateSettingsContainer", menuName = "Scriptable Objects/TranslateSettingsContainer")]
public class TranslateSettingsContainer : ScriptableObject
{
    [Header("__ Language settings __")]
    [Tooltip("Is using custom font asset?")]
    [SerializeField] internal bool isUsingFontAsset = false;
    [Tooltip("Custom font asset")]
    [SerializeField] internal TMP_FontAsset fontAsset;
    [Tooltip("Is need recalculate scale?")]
    [SerializeField] internal bool recalculateScale = false;
    [Tooltip("Custom text scaler. (1 = 100% of original size).")]
    [SerializeField] internal float scaler = 1.0f;
    [Tooltip("Is censored language?")]
    [SerializeField] internal bool isCensored = false;
}

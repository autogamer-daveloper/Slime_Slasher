using UnityEngine;

[CreateAssetMenu(fileName = "TranslateCentralSettings", menuName = "Scriptable Objects/TranslateCentralSettings")]
public class TranslateCentralSettings : ScriptableObject
{
    [Tooltip("Sorted by language id settings for translation.")]
    [SerializeField] internal TranslateSettingsContainer[] container;
}

using UnityEngine;

[CreateAssetMenu(fileName = "TranslateContainer", menuName = "Scriptable Objects/TranslateContainer")]
public class TranslateContainer : ScriptableObject
{
    [Header("__ Languages __")]
    public LocalizedText[] texts;
}

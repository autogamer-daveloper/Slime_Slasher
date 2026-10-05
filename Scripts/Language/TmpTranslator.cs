using UnityEngine;
using TMPro;

[System.Serializable]
public class LocalizedText
{
    [TextArea(5, 25)]
    public string text;
}

[RequireComponent(typeof(TMP_Text))]
public class TmpTranslator : MonoBehaviour
{
    [Header("__ Global settings __")]
    [Tooltip("Global settings container, which have all language's specific settings.")]
    [SerializeField] private TranslateCentralSettings globalSettings;
    [Header("__ Completed sample __")]
    [Tooltip("Text container.")]
    [SerializeField] private TranslateContainer container;

    private TMP_Text txt;

    private void Awake()
    {
        int lang = KeyManager.Get_Bool_Key("Language");
        txt = GetComponent<TMP_Text>();

        if (container != null && lang >= 0 && lang < container.texts.Length) { txt.text = container.texts[lang].text; }
        else { Debug.LogWarning($"[TmpTranslator] Missing translation for lang index {lang}", this); }

        if (globalSettings != null)
        {
            if(globalSettings.container[lang] == null) { Debug.LogError($"[TmpTranslator] Missing in global settings link to - container '{lang}'!"); return; }

            if (globalSettings.container[lang].isUsingFontAsset)
            {
                if (globalSettings.container[lang].fontAsset == null) { Debug.LogError($"[TmpTranslator] Missing in language settings '{lang}' link to - fontAsset!"); return; }
                else { txt.font = globalSettings.container[lang].fontAsset; }
            }
            if (globalSettings.container[lang].recalculateScale)
            {
                txt.fontSize *= globalSettings.container[lang].scaler;
            }
        }
        else { Debug.LogError($"[TmpTranslator] Missing global language's settings!"); }
    }
}
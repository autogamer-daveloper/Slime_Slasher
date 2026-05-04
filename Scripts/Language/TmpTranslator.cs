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
    [Header("__ Completed sample __")]
    [SerializeField] private TranslateContainer container;

    private TMP_Text txt;

    private void Awake()
    {
        int lang = KeyManager.Get_Bool_Key("Language");
        txt = GetComponent<TMP_Text>();

        if (container != null && lang >= 0 && lang < container.texts.Length) { txt.text = container.texts[lang].text; }
        else { Debug.LogWarning($"[TmpTranslator] Missing translation for lang index {lang}", this); }
    }
}
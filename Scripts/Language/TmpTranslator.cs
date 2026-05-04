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
    [Header("__ Languages __")]
    [SerializeField] private LocalizedText[] texts;

    private TMP_Text txt;

    private void Awake()
    {
        int lang = KeyManager.Get_Bool_Key("Language");
        txt = GetComponent<TMP_Text>();

        if (lang >= 0 && lang < texts.Length)
            txt.text = texts[lang].text;
    }
}
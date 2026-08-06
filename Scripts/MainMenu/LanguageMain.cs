using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LanguageMain : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private ChangeLangButton[] btn;
    [SerializeField] private GameObject loadLevel;

    private int _usingLang;

    private void Start()
    {
        _usingLang = KeyManager.Get_Bool_Key("Language");
        for (int i = 0; i < btn.Length; i++)
        {
            int index = i;
            btn[index].action = () => { ChangeLanguage(index); };
            btn[index].button.onClick.AddListener(btn[index].action);
        }
    }

    private void OnDestroy() { foreach (ChangeLangButton b in btn) { b.button.onClick.RemoveListener(b.action); } }

    private void ChangeLanguage(int id)
    {
        if (_usingLang == id) { return; }
        _usingLang = id;
        KeyManager.Set_Bool_Key("Language", _usingLang);

        loadLevel.SetActive(true);
        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene() { LoadLevel.LoadLevelById(0); }
}

[System.Serializable]
internal class ChangeLangButton
{
    public Button button;
    public UnityAction action;
}

using UnityEngine;
using UnityEngine.UI;

public class LanguageMain : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Button[] changeLanguage;
    [SerializeField] private GameObject loadLevel;

    private int _usingLang;
    private int _langCount = 2;

    private void Start()
    {
        _usingLang = KeyManager.Get_Bool_Key("Language");
        foreach (GameObject obj in buttons) { obj.SetActive(false); }
        buttons[_usingLang].SetActive(true);
        foreach (Button btn in changeLanguage) { btn.onClick.AddListener(ChangeLanguage); }
    }

    private void OnDestroy() { foreach (Button btn in changeLanguage) { btn.onClick.RemoveListener(ChangeLanguage); } }

    private void ChangeLanguage()
    {
        if (_usingLang < _langCount - 1) { _usingLang++; }
        else { _usingLang = 0; }
        KeyManager.Set_Bool_Key("Language", _usingLang);

        loadLevel.SetActive(true);
        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene() { LoadLevel.LoadLevelById(0); }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class SelectCharacterForZeroCreating : MonoBehaviour
{
    [Header("__ Characters __")]
    [SerializeField] private CharForZeroCreating[] characters;
    [Header("__ UI __")]
    [SerializeField] private Button select;
    [SerializeField] private GameObject selectingPanel;
    [Header("__ Animation __")]
    [SerializeField] private GameObject consoleObject;
    [SerializeField] private Animation consolePanel;
    [SerializeField] private string hideAllConsole = "Hide_all_console";
    [SerializeField] private Animation charPanel;
    [SerializeField] private string hideAllChar = "Hide_all_char";
    [SerializeField] private GameObject spawnAnim;
    [Header("__ Audio __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioSource glitchSrc;
    [SerializeField] private AudioSource ambientSrc;
    [SerializeField] private GameObject ambientObj;

    private bool _isFirstSelecting = true;
    private int _selectingChar = 0;
    private bool _isSelected = false;
    private int _selectedChar = 0;
    private bool _locked = false;

    //DOTween
    private Vector2 hiddenImage = new Vector2(500, 425);
    private Vector2 shownImage = new Vector2(-300, 425);

    private Vector2 minScale = new Vector2(1f, 1f);
    private Vector2 maxScale = new Vector2(1.1f, 1.1f);

    private void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            int index = i;
            characters[index].action = () => { ShowImage(index); };
            characters[index].button.onClick.AddListener(characters[index].action);
        }
        select.onClick.AddListener(SelectThisCharacter);
    }

    private void ShowImage(int id)
    {
        if (_isSelected) { return; }
        if (_locked) { return; }

        _selectingChar = id;
        if (_isFirstSelecting) { _isFirstSelecting = false; selectingPanel.SetActive(true); }
        for (int i = 0; i < characters.Length; i++)
        {
            int index = i;
            if (index == id)
            {
                characters[index].charName.SetActive(true);
                characters[index].image.DOAnchorPos(shownImage, 0.5f);
                characters[index].buttonRect.DOScale(maxScale, 0.25f);
            }
            else
            {
                characters[index].charName.SetActive(false);
                characters[index].image.DOAnchorPos(hiddenImage, 0.5f);
                characters[index].buttonRect.DOScale(minScale, 0.25f);
            }
        }

        _locked = true;
        Invoke(nameof(UnlockButtons), 0.5f);

        src.PlayOneShot(click);
    }

    private void UnlockButtons() { _locked = false; }

    private void OnDestroy()
    {
        foreach (CharForZeroCreating character in characters) { character.button.onClick.RemoveAllListeners(); }
        select.onClick.RemoveListener(SelectThisCharacter);
    }

    private void SelectThisCharacter()
    {
        _isSelected = true;
        _selectedChar = _selectingChar;
        consolePanel.Play(hideAllConsole);
        charPanel.Play(hideAllChar);
        spawnAnim.SetActive(true);
        Invoke(nameof(HideConsole), 0.5f);
        ambientObj.SetActive(true);
        ambientSrc.DOFade(1f, 3f);
        glitchSrc.DOFade(0f, 1.5f);
    }

    private void HideConsole()
    {
        characters[_selectedChar].character.SetActive(true);
        consoleObject.SetActive(false);
    }

    public void LoadNextScene() { LoadLevel.LoadLevelById(characters[_selectedChar].sceneId); }
}

[System.Serializable]
public class CharForZeroCreating
{
    public GameObject character;
    public GameObject charName;
    public Button button;
    public RectTransform buttonRect;
    public RectTransform image;
    public int sceneId = 0;
    [HideInInspector] public UnityAction action;
}

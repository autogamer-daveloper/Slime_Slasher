using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelLoading : MonoBehaviour
{
    [Header("__ Loading panels __")]
    [SerializeField] private GameObject loader;
    [SerializeField] private int[] sceneIndex;
    [Header("__ UI __")]
    [SerializeField] private RectTransform panelStartNew;
    [SerializeField] private RectTransform panelContinue;
    [SerializeField] private RectTransform panelExit;
    [Header("__ Show panels __")]
    [SerializeField] private Button exit;
    [SerializeField] private Button startNew;
    [SerializeField] private Button continueGame;
    [Header("__ Accept action __")]
    [SerializeField] private Button exitYes;
    [SerializeField] private Button startNewYes;
    [SerializeField] private Button continueGameYes;
    [SerializeField] private Toggle isEasyMode;
    [Header("__ Hide panels __")]
    [SerializeField] private Button[] hideAll;

    private int saveIndex = 0;
    private bool locked = false;
    private Vector2 hidden = new Vector2(0, -2000);
    private Vector2 shown = new Vector2(0, 0);

    private bool startWithEasyMode = false;

    private void Start()
    {
        Application.targetFrameRate = 60;

        saveIndex = KeyManager.Get_Bool_Key("gameEpisode");
        if (saveIndex == 0) { continueGame.interactable = false; }
        else { continueGame.interactable = true; }

        isEasyMode.onValueChanged.AddListener(delegate { OnToggleChanged(isEasyMode); });

        foreach (Button btn in hideAll) { btn.onClick.AddListener(HideAllPanels); }

        startNew.onClick.AddListener(ShowStart);
        continueGame.onClick.AddListener(ShowContinue);
        exit.onClick.AddListener(ShowExit);

        exitYes.onClick.AddListener(Exit);
        startNewYes.onClick.AddListener(StartNewGame);
        continueGameYes.onClick.AddListener(ContinueGame);
    }

    private void OnDestroy()
    {
        foreach (Button btn in hideAll) { btn.onClick.RemoveListener(HideAllPanels); }

        startNew.onClick.RemoveListener(ShowStart);
        continueGame.onClick.RemoveListener(ShowContinue);
        exit.onClick.RemoveListener(ShowExit);

        exitYes.onClick.RemoveListener(Exit);
        startNewYes.onClick.RemoveListener(StartNewGame);
        continueGameYes.onClick.RemoveListener(ContinueGame);

        isEasyMode.onValueChanged.RemoveAllListeners();
    }

    private void ShowStart() { panelStartNew.DOAnchorPos(shown, 0.5f); locked = true; }
    private void ShowContinue() { panelContinue.DOAnchorPos(shown, 0.5f); locked = true; }
    private void ShowExit() { panelExit.DOAnchorPos(shown, 0.5f); locked = true; }

    private void HideAllPanels()
    {
        panelStartNew.DOAnchorPos(hidden, 0.5f);
        panelContinue.DOAnchorPos(hidden, 0.5f);
        panelExit.DOAnchorPos(hidden, 0.5f);
    }

    private void UnlockByTimer(float time) { Invoke(nameof(UnlockInteractable), time); }
    private void UnlockInteractable() { locked = false; }

    private void Exit() { if (locked) return; Application.Quit(); }

    private void StartNewGame()
    {
        if (locked) return;
        PlayerPrefs.DeleteAll();
        KeyManager.Set_Bool_Key("gameEpisode", 0);
        saveIndex = 0;
        if(startWithEasyMode == true) { KeyManager.Set_Bool_Key("easyMode", 1); }
        else { KeyManager.Set_Bool_Key("easyMode", 0); }
        Invoke(nameof(LoadGame), 1f);
        loader.SetActive(true);
    }

    private void ContinueGame()
    {
        if (locked) return;
        Invoke(nameof(LoadGame), 1f);
        loader.SetActive(true);
    }

    private void LoadGame() { LoadLevel.LoadLevelById(sceneIndex[saveIndex]); }

    private void OnToggleChanged(Toggle change) { startWithEasyMode = change.isOn; }
}

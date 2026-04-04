using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    [Header("__ Buttons __")]
    [SerializeField] private Button pause;
    [SerializeField] private Button resume;
    [SerializeField] private Button menu;
    [Header("__ UI __")]
    [SerializeField] private GameObject loader;
    [SerializeField] private RectTransform panel;

    private Vector2 shown = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);

    private void Start()
    {
        pause.onClick.AddListener(Pause);
        resume.onClick.AddListener(Continue);
        menu.onClick.AddListener(Menu);

        pause.onClick.AddListener(TemporaryBlockActions);
        resume.onClick.AddListener(TemporaryBlockActions);
        menu.onClick.AddListener(TemporaryBlockActions);
    }

    private void OnDestroy()
    {
        pause.onClick.RemoveListener(Pause);
        resume.onClick.RemoveListener(Continue);
        menu.onClick.RemoveListener(Menu);

        pause.onClick.RemoveListener(TemporaryBlockActions);
        resume.onClick.RemoveListener(TemporaryBlockActions);
        menu.onClick.RemoveListener(TemporaryBlockActions);
    }

    private void Pause() { panel.DOAnchorPos(shown, 0.5f); Invoke(nameof(StopTime), 0.5f); }
    private void Continue() { panel.DOAnchorPos(hidden, 0.5f); PlayTime(); }
    private void Menu()
    {
        PlayTime();
        loader.SetActive(true);
        Invoke(nameof(_LoadLevel), 1f);
    }

    private void _LoadLevel()
    {
        LoadLevel.LoadLevelById(0);
    }

    private void StopTime() { Time.timeScale = 0; }
    private void PlayTime() { Time.timeScale = 1; }

    private void TemporaryBlockActions()
    {
        pause.interactable = false;
        resume.interactable = false;
        menu.interactable = false;

        Invoke(nameof(UnblockActions), 0.5f);
    }

    private void UnblockActions()
    {
        pause.interactable = true;
        resume.interactable = true;
        menu.interactable = true;
    }
}

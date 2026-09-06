using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class MainButtonsPanel : MonoBehaviour
{
    [Header("__ Panel __")]
    [SerializeField] private RectTransform mainButtonsPanel;
    [Header("__ Buttons __")]
    [SerializeField] private Button[] showPanelButtons;
    [SerializeField] private Button[] hidePanelButtons;
    [Header("__ Input type __")]
    [SerializeField] private InputType type;

    private Vector2 shown = new Vector2(0, 100);
    private Vector2 hidden = new Vector2(0, -300);

    private Vector2 shownPC = new Vector2(-250, 375);
    private Vector2 hiddenPC = new Vector2(250, 375);

    private UnityAction show;
    private UnityAction hide;

    private void Start()
    {
        show = () => { MovePanel(true); };
        hide = () => { MovePanel(false); };

        foreach(Button btn in showPanelButtons) { btn.onClick.AddListener(show); }
        foreach(Button btn in hidePanelButtons) { btn.onClick.AddListener(hide); }
    }

    private void OnDestroy()
    {
        foreach(Button btn in showPanelButtons) { btn.onClick.RemoveListener(show); }
        foreach(Button btn in hidePanelButtons) { btn.onClick.RemoveListener(hide); }
    }

    private void MovePanel(bool showPanel)
    {
        bool isMobile = type.IsMobileInput();
        Vector2 target = new Vector2(0, 0);
        if (isMobile) { target = showPanel ? shown : hidden; }
        else { target = showPanel ? shownPC : hiddenPC; }
        mainButtonsPanel.DOKill();
        mainButtonsPanel.DOAnchorPos(target, 0.5f);
    }
}

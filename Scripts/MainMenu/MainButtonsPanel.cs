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

    private Vector2 shown = new Vector2(0, 100);
    private Vector2 hidden = new Vector2(0, -300);

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
        Vector2 target = showPanel ? shown : hidden;
        mainButtonsPanel.DOKill();
        mainButtonsPanel.DOAnchorPos(target, 0.5f);
    }
}

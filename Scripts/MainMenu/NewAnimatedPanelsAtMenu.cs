using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class NewAnimatedPanelsAtMenu : MonoBehaviour
{
    [Header("__ Panels __")]
    [SerializeField] private AnimatedPanelForNewMenu[] panels;
    [Header("__ Time __")]
    [SerializeField] private float timer = 0.5f;
    [Header("__ Background animations __")]
    [SerializeField] private Animation logo;
    [SerializeField] private Animation shadowPanel;

    private const string _hide_logo = "New_logo_hide";
    private const string _show_logo = "New_logo_show";

    private const string _hide_shadow_panel = "Menu_BG_shadow_hide";
    private const string _show_shadow_panel = "Menu_BG_shadow_show";

    private Vector2 hidden = new Vector2(0, -2000);
    private Vector2 shown = new Vector2(0, 0);

    private int _using = -1;
    private bool _isHiddenLogo = false;

    private void Start()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            int index = i;
            panels[index].show = () => { ShowPanel(index); };
            panels[index].hide = () => { HidePanel(index); };
            panels[index].buttonShow.onClick.AddListener(panels[index].show);
            panels[index].buttonHide.onClick.AddListener(panels[index].hide);
        }
    }

    private void OnDestroy()
    {
        foreach (AnimatedPanelForNewMenu pan in panels)
        {
            pan.buttonShow.onClick.RemoveAllListeners();
            pan.buttonHide.onClick.RemoveAllListeners();
        }
    }

    private void ShowPanel(int id)
    {
        _using = id;
        Background(false);
        panels[id].panel.DOAnchorPos(shown, timer);
    }

    private void HidePanel(int id)
    {
        if (_using == id)
        {
            _using = -1;
            Background(true);
            foreach (AnimatedPanelForNewMenu pan in panels) { pan.panel.DOAnchorPos(hidden, timer); }
        }
    }

    private void Background(bool show)
    {
        if (show && _isHiddenLogo == true)
        {
            logo.Play(_show_logo);
            shadowPanel.Play(_hide_shadow_panel);
            _isHiddenLogo = false;
        }
        else if (_isHiddenLogo == false)
        {
            logo.Play(_hide_logo);
            shadowPanel.Play(_show_shadow_panel);
            _isHiddenLogo = true;
        }
    }
}

[System.Serializable]
internal class AnimatedPanelForNewMenu
{
    public RectTransform panel;
    public Button buttonShow;
    public Button buttonHide;
    public UnityAction show;
    public UnityAction hide;
}

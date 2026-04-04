using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Newspaper : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button[] switchPanel;

    private bool isActive = false;
    private Vector2 hidden = new Vector2(0, -2000); 
    private Vector2 shown = new Vector2(0, 0); 

    private void Start()
    {
        foreach (Button btn in switchPanel)
        {
            btn.onClick.AddListener(Switch);
        }
    }

    private void OnDestroy()
    {
        foreach (Button btn in switchPanel)
        {
            btn.onClick.RemoveListener(Switch);
        }
    }

    private void Switch()
    {
        foreach (Button btn in switchPanel) { btn.interactable = false; }
        if (isActive)
        {
            panel.DOAnchorPos(hidden, 1f).OnComplete(() => { foreach (Button btn in switchPanel) { btn.interactable = true; } });
        }
        else
        {
            panel.DOAnchorPos(shown, 1f).OnComplete(() => { foreach (Button btn in switchPanel) { btn.interactable = true; } });
        }
        isActive = !isActive;
    }
}

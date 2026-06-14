using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class InventoryInfoPanels : MonoBehaviour
{
    [Header("__ Panels __")]
    [SerializeField] private RectTransform[] panels;
    [Header("__ Buttons __")]
    [SerializeField] private Button autoHideAll;
    [Header("__ Settings __")]
    [SerializeField] private Vector2 shown = new Vector2(0, 0);
    [SerializeField] private Vector2 hidden = new Vector2(0, -2000);
    [SerializeField] private float timer = 0.5f;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip click;

    private void Start() { autoHideAll.onClick.AddListener(AutoHideAll); }
    private void OnDestroy() { autoHideAll.onClick.RemoveListener(AutoHideAll); }

    private void AutoHideAll() { foreach (RectTransform panel in panels) { panel.DOAnchorPos(hidden, timer); }}

    public void OpenPanel(int id)
    {
        panels[id].gameObject.SetActive(true);
        panels[id].DOAnchorPos(shown, timer);
        src.PlayOneShot(click);
    }

    public void ClosePanel(int id)
    {
        panels[id].DOAnchorPos(hidden, timer).OnComplete(() => { panels[id].gameObject.SetActive(false); });
        src.PlayOneShot(click);
    }
}
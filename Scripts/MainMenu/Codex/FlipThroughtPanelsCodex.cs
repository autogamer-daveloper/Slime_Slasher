using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlipThroughtPanelsCodex : MonoBehaviour
{
    [Tooltip("Select all using panels. First of them will shown at start.")]
    [SerializeField] private GameObject[] panels;
    [Tooltip("Show next panel. May use one button.")]
    [SerializeField] private Button next;
    [Tooltip("Show previous panel. May use one button.")]
    [SerializeField] private Button previous;

    private UnityAction _next;
    private UnityAction _previous;

    private int _using = 0;

    private void Start()
    {
        _next = () =>
        {
            if (_using < panels.Length - 1) { _using += 1; }
            else { _using = 0; }
            ShowPanel(_using);
        };

        _previous = () =>
        {
            if (_using > 0) { _using -= 1; }
            else { _using = panels.Length - 1; }
            ShowPanel(_using);
        };

        if(next != null) { next.onClick.AddListener(_next); }
        if(previous != null) { previous.onClick.AddListener(_previous); }
    }

    private void OnDestroy()
    {
        if(next != null) { next.onClick.RemoveListener(_next); }
        if(previous != null) { previous.onClick.RemoveListener(_previous); }
    }

    private void ShowPanel(int id)
    {
        foreach (GameObject obj in panels) { obj.SetActive(false); }
        panels[id].SetActive(true);
    }
}

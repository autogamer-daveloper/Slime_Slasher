using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchPanelsCodex : MonoBehaviour
{
    [Tooltip("Select all using panels and their buttons for selecting. First of them will shown at start.")]
    [SerializeField] private SwitchPanelsCodexCase[] panels;

    private int _using = -1;

    private void Start()
    {
        ShowPanel(0);

        for (int i = 0; i < panels.Length; i++)
        {
            int id = i;
            panels[id].action = () => { ShowPanel(id); };
            panels[id].select.onClick.AddListener(panels[id].action);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            int id = i;
            panels[id].select.onClick.RemoveListener(panels[id].action);
        }
    }

    private void ShowPanel(int id)
    {
        if(_using == id) { return; }
        foreach (SwitchPanelsCodexCase obj in panels) { obj.panel.SetActive(false); }
        panels[id].panel.SetActive(true);
    }
}

[System.Serializable]
internal class SwitchPanelsCodexCase
{
    public GameObject panel;
    public Button select;
    internal UnityAction action;
}

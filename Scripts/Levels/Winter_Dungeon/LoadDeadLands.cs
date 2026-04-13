using UnityEngine;
using UnityEngine.UI;

public class LoadDeadLands : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private Button escape;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private int index = 7;

    private void Start()
    {
        escape.onClick.AddListener(Escape);
    }

    private void OnDestroy()
    {
        escape.onClick.RemoveListener(Escape);
    }

    private void Escape()
    {
        loadPanel.SetActive(true);
        KeyManager.Set_Bool_Key("gameEpisode", 4);
        Invoke(nameof(_Escape), 10f);
    }

    private void _Escape()
    {
        LoadLevel.LoadLevelById(index);
    }
}

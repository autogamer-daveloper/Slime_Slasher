using UnityEngine;

public class LoadWinter : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private int index = 5;

    public void Escape()
    {
        loadPanel.SetActive(true);
        KeyManager.Set_Bool_Key("gameEpisode", 3);
        Invoke(nameof(_Escape), 1f);
    }

    private void _Escape()
    {
        LoadLevel.LoadLevelById(index);
    }
}

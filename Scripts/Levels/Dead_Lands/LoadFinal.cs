using UnityEngine;

public class LoadFinal : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private int index = 8;
    [SerializeField] private Animation penguin;

    public void Escape()
    {
        penguin.Play();
        loadPanel.SetActive(true);
        KeyManager.Set_Bool_Key("gameEpisode", 5);
        Invoke(nameof(_Escape), 1.5f);
    }

    private void _Escape() { LoadLevel.LoadLevelById(index); }
}

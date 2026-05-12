using UnityEngine;

public class EndGameCutsceneLoad : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private int indexCanon = 11;
    [SerializeField] private int indexAlternate = 12;
    [SerializeField] private GameObject gameplayCamera;
    [SerializeField] private GameObject cutsceneCamera;

    public void EndGame(bool isCanon)
    {
        loadPanel.SetActive(true);
        gameplayCamera.SetActive(false);
        cutsceneCamera.SetActive(true);
        if (isCanon) { Invoke(nameof(_EscapeCanon), 1f); }
        else { Invoke(nameof(_EscapeAlternate), 1f); }
    }

    private void _EscapeCanon() { LoadLevel.LoadLevelById(indexCanon); }
    private void _EscapeAlternate() { LoadLevel.LoadLevelById(indexAlternate); }
}

using UnityEngine;

public class NightmareManager : MonoBehaviour
{
    [Header("__ If you dead __")]
    [SerializeField] private GameObject _activateDead;
    [SerializeField] private int sceneIdDead = 3;
    [Header("__ If you alive __")]
    [SerializeField] private GameObject _activateAlive;
    [SerializeField] private int sceneIdAlive = 2;

    public void Dead()
    {
        _activateDead.SetActive(true);
        Invoke(nameof(_Dead), 5);
    }

    private void _Dead() { LoadLevel.LoadLevelById(sceneIdDead); }

    public void Alive()
    {
        _activateAlive.SetActive(true);
        KeyManager.Set_Bool_Key("gameEpisode", 1);
        Invoke(nameof(_Alive), 1);
    }

    private void _Alive() { LoadLevel.LoadLevelById(sceneIdAlive); }
}

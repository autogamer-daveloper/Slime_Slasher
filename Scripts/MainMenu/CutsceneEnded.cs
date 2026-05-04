using UnityEngine;

public class CutsceneEnded : MonoBehaviour
{
    [SerializeField] private int sceneId = 0;

    public void LoadScene() { LoadLevel.LoadLevelById(sceneId); }
}

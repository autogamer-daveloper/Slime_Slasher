using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    internal static void LoadLevelById(int id)
    {
        SceneManager.LoadScene(id);
    }
}

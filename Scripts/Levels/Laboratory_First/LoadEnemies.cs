using UnityEngine;
using UnityEngine.UI;

public class LoadEnemies : MonoBehaviour
{
    [Header("__ Enemies __")]
    [SerializeField] private GameObject[] enemies;
    [Header("__ UI __")]
    [SerializeField] private Button loadButton;

    private void Start() { loadButton.onClick.AddListener(Load); }
    private void OnDestroy() { loadButton.onClick.RemoveListener(Load); }

    private void Load() { foreach(GameObject obj in enemies) { obj.SetActive(true); } }
}

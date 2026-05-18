using UnityEngine;
using UnityEngine.UI;

public class CheatLevelLoader : MonoBehaviour
{
    [SerializeField] private Button btn;

    private void Start() { btn.onClick.AddListener(Click); }
    private void OnDestroy() { btn.onClick.RemoveListener(Click); }

    private void Click() { LoadLevel.LoadLevelById(14); }
}

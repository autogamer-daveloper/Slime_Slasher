using UnityEngine;
using UnityEngine.UI;

public class EnemySlimeKilling : MonoBehaviour
{
    [Header("__ Setting __")]
    [SerializeField] private string keyName = "isSlimePrisonerKilled";
    [SerializeField] private Button SetKeyButton;

    private bool isKilled = false;

    private void Start() { SetKeyButton.onClick.AddListener(SetKey); }

    private void OnDestroy() { SetKeyButton.onClick.RemoveListener(SetKey); }

    public void Kill() { isKilled = true; }

    private void SetKey()
    {
        int _isKilled = 0;
        if (isKilled == true) _isKilled = 1;
        KeyManager.Set_Bool_Key(keyName, _isKilled);
    }
}

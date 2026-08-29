using UnityEngine;
using UnityEngine.InputSystem;

public class HideInterface : MonoBehaviour
{
    [Header("__ All UI __")]
    [Tooltip("Select every User Interface element for hide them by click F1 on keyboard. Only for debug/screen record or something like that.")]
    [SerializeField] private GameObject[] ui;
    private bool _uiVisible = true;

    private void Update() { if (Keyboard.current.f1Key.wasPressedThisFrame) { SwitchInterface(); }}

    private void SwitchInterface()
    {
        _uiVisible = !_uiVisible;
        foreach (GameObject obj in ui) { obj.SetActive(_uiVisible); }
    }
}

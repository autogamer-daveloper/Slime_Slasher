using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DelayStartTimer : MonoBehaviour
{
    [Header("__ Delay __")]
    [SerializeField] private float delay;
    [SerializeField] private UnityEvent action;
    [SerializeField] private bool isLoadLevelAction = false;
    [SerializeField] private int index = 0;
    [Tooltip("Optional. May be used for skip")]
    [SerializeField] private Button CancelInvokeButton;
    [SerializeField] private bool doTask = false;
    [SerializeField] private bool doubleClick = false;

    private bool _isFirstClicked = false;

    private void Start() { if (CancelInvokeButton != null) CancelInvokeButton.onClick.AddListener(Cancel); }

    private void OnDestroy() { if (CancelInvokeButton != null) CancelInvokeButton.onClick.RemoveListener(Cancel); }

    private void OnEnable()
    {
        if (isLoadLevelAction == false) { Invoke(nameof(Action), delay); }
        else { Invoke(nameof(ActionLevelLoad), delay); }
    }

    private void Action() { action.Invoke(); }

    private void ActionLevelLoad() { LoadLevel.LoadLevelById(index); }

    private void Cancel()
    {
        if (doubleClick && !_isFirstClicked) { _isFirstClicked = true; Invoke(nameof(CancelDoubleClick), 5f); return; }

        if (isLoadLevelAction == false) { CancelInvoke(nameof(Action)); }
        else { CancelInvoke(nameof(ActionLevelLoad)); }

        if (doTask)
        {
            if (isLoadLevelAction == false) { Action(); }
            else { ActionLevelLoad(); }
        }
    }
    
    private void CancelDoubleClick() { _isFirstClicked = false; }
}

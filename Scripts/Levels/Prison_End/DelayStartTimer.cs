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
    [Header("__ Skip button __")]
    [Tooltip("Optional. May be used for skip")]
    [SerializeField] private Button CancelInvokeButton;
    [SerializeField] private bool doTask = false;
    [Header("__ AudioSources for stop dialogue __")]
    [Tooltip("Optional. For stop dialogue sfx.")]
    [SerializeField] private AudioSource dialogueSFX;

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
        if(dialogueSFX != null) { dialogueSFX.Stop(); }

        if (isLoadLevelAction == false) { CancelInvoke(nameof(Action)); }
        else { CancelInvoke(nameof(ActionLevelLoad)); }

        if (doTask)
        {
            if (isLoadLevelAction == false) { Action(); }
            else { ActionLevelLoad(); }
        }
    }
}

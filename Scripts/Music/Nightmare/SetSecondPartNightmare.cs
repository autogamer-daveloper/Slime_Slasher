using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetSecondPartNightmare : MonoBehaviour
{
    [Header("__ Audio Sources __")]
    [SerializeField] private AudioSource _1;
    [SerializeField] private AudioSource _2;
    [SerializeField] private GameObject _2Obj;
    [SerializeField] private AudioSource[] needMute;
    [Header("__ UI __")]
    [SerializeField] private Button activateSecond;
    [Header("__ Volume __")]
    [SerializeField] private float volume = 0.25f;
    [Header("__ First Muted Time __")]
    [SerializeField] private float fmt = 0.1f;

    private void Start() { activateSecond.onClick.AddListener(SetSecond); Invoke(nameof(UnmuteFirst), fmt); }
    private void OnDestroy() { activateSecond.onClick.RemoveListener(SetSecond); }

    private void UnmuteFirst() { _1.DOFade(volume, 0.5f); }

    private void SetSecond()
    {
        _1.DOFade(0f, 0.5f);
        _2Obj.SetActive(true);
        _2.DOFade(volume, 0.5f);
    }

    public void MuteAll() { _2.DOFade(0f, 2f); foreach(AudioSource mute in needMute) { mute.DOFade(0f, 2f); } }
}

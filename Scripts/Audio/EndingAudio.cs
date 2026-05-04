using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EndingAudio : MonoBehaviour
{
    [Header("__ Audio __")]
    [SerializeField] private AudioSource endingMusic;
    [SerializeField] private UnityEvent Action;
    [SerializeField] private float duration = 60f;
    [SerializeField] private float volume = 0.5f;

    private void OnEnable() { CancelInvoke(nameof(Delay)); endingMusic.Play(); endingMusic.DOFade(volume, 5f); Invoke(nameof(Delay), duration); }
    private void Delay() { endingMusic.DOFade(0f, 5f).OnComplete(() => { Action.Invoke(); }); }
}

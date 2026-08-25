using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EndingAudio : MonoBehaviour
{
    [Header("__ Audio __")]
    [Tooltip("Source of ending music.")]
    [SerializeField] private AudioSource endingMusic;
    [Tooltip("What to do after activation.")]
    [SerializeField] private UnityEvent Action;
    [Tooltip("Ending duration.")]
    [SerializeField] private float duration = 60f;
    [Tooltip("Original volume (Before calculating).")]
    [SerializeField] private float volume = 0.5f;

    private const string audioVolumeKey = "AudioVolume";
    private float volumeCalculated = 0.5f;

    private void OnEnable()
    {
        CancelInvoke(nameof(Delay));
        float volumeLevel = KeyManager.Get_Bool_Key(audioVolumeKey);
        volumeCalculated = volume * (volumeLevel / 100f);
        endingMusic.Play();
        endingMusic.DOFade(volumeCalculated, 5f).SetAutoKill(true);
        Invoke(nameof(Delay), duration);
    }
    private void Delay()
    {
        endingMusic.DOFade(0f, 5f).SetAutoKill(true).OnComplete(() => { Action.Invoke(); });
    }
}

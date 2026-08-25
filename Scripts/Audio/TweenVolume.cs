using UnityEngine;
using DG.Tweening;

public class TweenVolume : MonoBehaviour
{
    [Tooltip("Set audioSource, which volume value you want change.")]
    [SerializeField] private AudioSource src;
    [Tooltip("Set start volume value.")]
    [SerializeField] private float startVolumeValue = 0.15f;
    [Tooltip("Set end volume value.")]
    [SerializeField] private float endVolumeValue = 0.5f;
    [Header("__ Fade time __")]
    [Tooltip("Set fade time")]
    [SerializeField] private float fadeTime = 0.5f;

    private const string audioVolumeKey = "AudioVolume";
    private float endVolumeCalculated = 0.25f;

    private void Awake()
    {
        src.volume = startVolumeValue;
        float volumeLevel = KeyManager.Get_Bool_Key(audioVolumeKey);
        endVolumeCalculated = endVolumeValue * (volumeLevel / 100f);
    }

    public void SetNewValue()
    {
        src.DOKill();
        src.DOFade(endVolumeCalculated, fadeTime);
    }

    public void SetOldValue()
    {
        src.DOKill();
        src.DOFade(startVolumeValue, fadeTime);
    }
}

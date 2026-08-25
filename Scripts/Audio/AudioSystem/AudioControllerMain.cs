using UnityEngine;
using DG.Tweening;

public class AudioControllerMain : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [Tooltip("Select audio source, which you want to turn on/off.")]
    [SerializeField] private AudioSource source;
    [Tooltip("Original volume (Before calculating).")]
    [SerializeField] private float volume = 0.5f;

    private const string audioVolumeKey = "AudioVolume";
    private float volumeCalculated = 0.5f;

    private void Start()
    {
        float volumeLevel = KeyManager.Get_Bool_Key(audioVolumeKey);
        volumeCalculated = volume * (volumeLevel / 100f);
    }

    internal void TurnOn() { source.DOFade(volumeCalculated, 1f); }
    internal void TurnOff() { source.DOFade(0f, 1f); }
}

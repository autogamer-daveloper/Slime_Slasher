using UnityEngine;
using System.Collections.Generic;

public class MicroAudioVolumeSetting : MonoBehaviour
{
    private const string audioVolumeKey = "AudioVolume";

    private void Awake() { CalculateVolume(); }

    private AudioSource GetAudioSource() { return this.gameObject.GetComponent<AudioSource>(); }

    private void CalculateVolume()
    {
        float volumeInSettings = 100f;

        if (PlayerPrefs.HasKey(audioVolumeKey)) { volumeInSettings = KeyManager.Get_Bool_Key(audioVolumeKey); }

        float multiplier = volumeInSettings / 100f;
        AudioSource src = GetAudioSource();
        if(src == null) { Debug.LogError($"[MicroAudioVolumeSetting.cs]: can't find audio source on object {this.gameObject.name}"); return; }
        float originalVolume = src.volume;
        src.volume = originalVolume * multiplier;

        Debug.Log($"Volume setting: {volumeInSettings}");
        Debug.Log($"Multiplier: {multiplier}");
    }

    internal void Recalculate() { CalculateVolume(); }
}
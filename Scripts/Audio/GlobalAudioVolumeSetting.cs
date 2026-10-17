using UnityEngine;
using System.Collections.Generic;

public class GlobalAudioVolumeSetting : MonoBehaviour
{
    [Header("Audio Source Exceptions")]
    [Tooltip("Select audio sources, which won't change volume by this class. (Has more priority then included field).")]
    [SerializeField] private AudioSource[] excludedAudioSources;
    [Tooltip("Select all inactive audio sources there, because only active audio sources will be found (without included field) at awake.")]
    [SerializeField] private AudioSource[] includedAudioSources;

    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();
    private const string audioVolumeKey = "AudioVolume";

    private void Awake() { CalculateVolume(); }

    private AudioSource[] FindAllAudioSources() { return FindObjectsByType<AudioSource>(FindObjectsSortMode.None); }

    private bool IsExcluded(AudioSource audioSource)
    {
        return excludedAudioSources != null && System.Array.IndexOf(excludedAudioSources, audioSource) >= 0;
    }

    private void CalculateVolume()
    {
        float volumeInSettings = 100f;

        if (PlayerPrefs.HasKey(audioVolumeKey)) { volumeInSettings = KeyManager.Get_Bool_Key(audioVolumeKey); }

        float multiplier = volumeInSettings / 100f;

        AudioSource[] allFoundAudioSources = FindAllAudioSources();

        foreach (AudioSource audioSource in allFoundAudioSources)
        {
            if (audioSource == null || IsExcluded(audioSource)) { continue; }
            if (!originalVolumes.ContainsKey(audioSource)) { originalVolumes.Add(audioSource, audioSource.volume); }
            audioSource.volume = originalVolumes[audioSource] * multiplier;
        }

        if (includedAudioSources != null && includedAudioSources.Length > 0)
        {
            foreach (AudioSource audioSource in includedAudioSources)
            {
                if (audioSource == null || IsExcluded(audioSource)) { continue; }
                if (!originalVolumes.ContainsKey(audioSource)) { originalVolumes.Add(audioSource, audioSource.volume); }
                audioSource.volume = originalVolumes[audioSource] * multiplier;
            }
        }

        Debug.Log($"Found AudioSources: {allFoundAudioSources.Length}");
        Debug.Log($"Volume setting: {volumeInSettings}");
        Debug.Log($"Multiplier: {multiplier}");
    }

    internal void Recalculate() { CalculateVolume(); }
}
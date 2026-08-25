using UnityEngine;
using DG.Tweening;

public class MainMusicController : MonoBehaviour
{
    [Header("__ Music settings __")]
    [Tooltip("Audio sources with selected music.")]
    [SerializeField] private AudioSource[] compositions;
    [Tooltip("Original volume (Before calculating).")]
    [SerializeField] private float volume = 0.5f;
    [Tooltip("Default music id. Scene will start with music by this id.")]
    [SerializeField] private int defaultMusicId = 0;

    private int _usingMusic = -1;
    private bool _isBlocked = true;
    private const string audioVolumeKey = "AudioVolume";
    private float volumeCalculated = 0.5f;

    private void Start()
    {
        float volumeLevel = KeyManager.Get_Bool_Key(audioVolumeKey);
        volumeCalculated = volume * (volumeLevel / 100f);
        Unlock();
    }

    public void SetAudio(int id)
    {
        if (_isBlocked) { return; }
        if (id == _usingMusic) { return; }
        for (int i = 0; i < compositions.Length; i++)
        {
            int index = i;
            if (index == id) { compositions[index].Stop(); compositions[index].Play(); compositions[index].DOFade(volumeCalculated, 2f); }
            else { compositions[index].DOFade(0f, 2f); }
        }
        _usingMusic = id;
    }

    public void MuteMusic() { foreach (AudioSource src in compositions) { src.DOFade(0f, 2f); } _usingMusic = -1; }
    public void Dead() { foreach (AudioSource src in compositions) { src.DOFade(0f, 0.5f); } _isBlocked = true; Invoke(nameof(Unlock), 6f); }
    
    private void Unlock() { _isBlocked = false; SetAudio(defaultMusicId); }
}

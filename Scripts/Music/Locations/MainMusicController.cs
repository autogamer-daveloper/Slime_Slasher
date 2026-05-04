using UnityEngine;
using DG.Tweening;

public class MainMusicController : MonoBehaviour
{
    [Header("__ Music settings __")]
    [SerializeField] private AudioSource[] compositions;
    [SerializeField] private float volume = 0.5f;
    [SerializeField] private int defaultMusicId = 0;

    private int _usingMusic = -1;
    private bool _isBlocked = false;

    private void Start() { Unlock(); }

    public void SetAudio(int id)
    {
        if (_isBlocked) { return; }
        if (id == _usingMusic) { return; }
        for (int i = 0; i < compositions.Length; i++)
        {
            int index = i;
            if (index == id) { compositions[index].Stop(); compositions[index].Play(); compositions[index].DOFade(volume, 2f); }
            else { compositions[index].DOFade(0f, 2f); }
        }
        _usingMusic = id;
    }

    public void MuteMusic() { foreach (AudioSource src in compositions) { src.DOFade(0f, 2f); } _usingMusic = -1; }
    public void Dead() { foreach (AudioSource src in compositions) { src.DOFade(0f, 0.5f); } _isBlocked = true; Invoke(nameof(Unlock), 6f); }
    
    private void Unlock() { _isBlocked = false; SetAudio(defaultMusicId); }
}

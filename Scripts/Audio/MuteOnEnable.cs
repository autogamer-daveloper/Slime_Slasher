using UnityEngine;
using DG.Tweening;

public class MuteOnEnable : MonoBehaviour
{
    [SerializeField] private AudioSource[] src;
    [SerializeField] private float volume = 0f;
    [SerializeField] private float time = 1f;
    [SerializeField] private bool isOnEnable = true;

    private void OnEnable() { if (isOnEnable) { MuteAllSources(); }}
    
    public void Mute() { MuteAllSources(); }

    private void MuteAllSources() { foreach (AudioSource s in src) { s.DOFade(volume, time); }}
}

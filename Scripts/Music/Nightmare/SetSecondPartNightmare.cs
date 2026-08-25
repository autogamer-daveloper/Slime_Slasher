using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetSecondPartNightmare : MonoBehaviour
{
    [Header("__ Audio Sources __")]
    [Tooltip("First audio source with first part of nightmare music.")]
    [SerializeField] private AudioSource _1;
    [Tooltip("Second audio source with second part of nightmare music.")]
    [SerializeField] private AudioSource _2;
    [Tooltip("Second audio source in GameObject field type. (Yes you can use '_2.gameObject' instead of my '_2Obj'...)")]
    [SerializeField] private GameObject _2Obj;
    [Tooltip("Which audio sources need to mute, when called method 'MuteAll'.")]
    [SerializeField] private AudioSource[] needMute;
    [Header("__ UI __")]
    [Tooltip("Select button, which click should 'summon' second part of nightmare music.")]
    [SerializeField] private Button activateSecond;
    [Header("__ Volume __")]
    [Tooltip("Original volume (Before calculating).")]
    [SerializeField] private float volume = 0.25f;
    [Header("__ First Muted Time __")]
    [Tooltip("Delay before first part will be 'summoned' (Counting from scene start).")]
    [SerializeField] private float fmt = 0.1f;
    
    private const string audioVolumeKey = "AudioVolume";
    private float volumeCalculated = 0.5f;

    private void Awake()
    {
        float volumeLevel = KeyManager.Get_Bool_Key(audioVolumeKey);
        volumeCalculated = volume * (volumeLevel / 100f);
    }

    private void Start() { DOTween.Clear(true); activateSecond.onClick.AddListener(SetSecond); Invoke(nameof(UnmuteFirst), fmt); }
    private void OnDestroy() { DOTween.Clear(true); activateSecond.onClick.RemoveListener(SetSecond); }

    private void UnmuteFirst() { _1.DOFade(volumeCalculated, 0.5f); }

    private void SetSecond()
    {
        _1.DOFade(0f, 0.5f);
        _2Obj.SetActive(true);
        _2.DOFade(volumeCalculated, 0.5f);
    }

    public void MuteAll() { _2.DOFade(0f, 0.25f).SetAutoKill(true); foreach(AudioSource mute in needMute) { if (mute != null) { mute.DOFade(0f, 0.25f).SetAutoKill(true); }}}
}

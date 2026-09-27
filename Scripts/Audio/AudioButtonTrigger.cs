using UnityEngine;
using UnityEngine.UI;

public class AudioButtonTrigger : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [Tooltip("Button, which on click will be activated sound effect.")]
    [SerializeField] private Button button;
    [Tooltip("Audio source for sound effect.")]
    [SerializeField] private AudioSource src;
    [Tooltip("Audio clip of sound effect.")]
    [SerializeField] private AudioClip sfx;
    [Tooltip("If you need to use public method.")]
    [SerializeField] private bool isPrivate = true;

    private void Start() { if (isPrivate) { button.onClick.AddListener(PlaySound); }}
    private void OnDestroy() { if (isPrivate) { button.onClick.RemoveListener(PlaySound); }}

    private void PlaySound() { src.PlayOneShot(sfx); }
    public void PlayThisSound() { if (isPrivate) { return; } PlaySound(); }
}

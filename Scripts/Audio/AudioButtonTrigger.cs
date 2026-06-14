using UnityEngine;
using UnityEngine.UI;

public class AudioButtonTrigger : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [SerializeField] private Button button;
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private bool isPrivate = true;

    private void Start() { if (isPrivate) { button.onClick.AddListener(PlaySound); }}
    private void OnDestroy() { if (isPrivate) { button.onClick.RemoveListener(PlaySound); }}

    private void PlaySound() { src.PlayOneShot(sfx); }
    public void PlayThisSound() { if (isPrivate) { return; } PlaySound(); }
}

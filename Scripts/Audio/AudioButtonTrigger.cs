using UnityEngine;
using UnityEngine.UI;

public class AudioButtonTrigger : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [SerializeField] private Button button;
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip sfx;

    private void Start() { button.onClick.AddListener(PlaySound); }
    private void OnDestroy() { button.onClick.RemoveListener(PlaySound); }

    private void PlaySound() { src.PlayOneShot(sfx); }
}

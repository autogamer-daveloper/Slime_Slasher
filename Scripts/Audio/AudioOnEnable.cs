using UnityEngine;

public class AudioOnEnable : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private bool isNeedDelay = false;
    [SerializeField] private float delay = 1f;

    private void Start()
    {
        if (isNeedDelay) { Invoke(nameof(PlaySound), delay); }
        else { PlaySound(); }
    }

    private void PlaySound() { src.PlayOneShot(sfx); }
}

using UnityEngine;

public class CutsceneSFX : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip[] sfx;

    public void PlaySFX(int id) { src.PlayOneShot(sfx[id]); }
}

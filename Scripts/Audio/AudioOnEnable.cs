using UnityEngine;

public class AudioOnEnable : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private bool isNeedDelay = false;
    [SerializeField] private float delay = 1f;
    [Header("__ Spec lang __")]
    [SerializeField] private bool isSpecLang = false;
    [SerializeField] private int usingLang = 1;

    private void Start()
    {
        if (isNeedDelay) { Invoke(nameof(PlaySound), delay); }
        else { PlaySound(); }
    }

    private void PlaySound()
    {
        if (isSpecLang)
        {
            int langUsing = KeyManager.Get_Bool_Key("Language");
            if (usingLang != langUsing) return;            
        }

        src.PlayOneShot(sfx);
    }
}

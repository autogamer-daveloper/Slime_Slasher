using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip whoosh;
    [Space(10)]
    [Header("__ Voices: Default __")]
    [SerializeField] private AudioClip defaultClip;
    [Header("__ Voices: English __")]
    [SerializeField] private AudioClip[] engClip;
    [Header("__ Voices: Russian __")]
    [SerializeField] private AudioClip[] rusClip;

    private int _usingLanguage;
    private int _usingVoiceId;
    private bool _isSaying = false;

    private bool _Debug = true;

    private void Start() { _usingLanguage = KeyManager.Get_Bool_Key("Language"); }

    public void SayText(int id)
    {
        if (_isSaying) { return; }
        src.PlayOneShot(whoosh);
        _usingVoiceId = id;
        _isSaying = true;
        Invoke(nameof(_SayText), 0.5f);
    }

    private void _SayText()
    {
        if (_Debug) { src.PlayOneShot(defaultClip); _usingVoiceId = -1; _isSaying = false; return; }

        switch (_usingLanguage)
        {
            case 0: src.PlayOneShot(engClip[_usingVoiceId]); break;
            case 1: src.PlayOneShot(rusClip[_usingVoiceId]); break;
            default: src.PlayOneShot(defaultClip); break;
        }

        _isSaying = false;
        _usingVoiceId = -1;
    }
}

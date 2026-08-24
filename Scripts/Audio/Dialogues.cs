using UnityEngine;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    [Header("__ Dialogue Animation __")]
    [Tooltip("Need to connect with dialogue animation component")]
    [SerializeField] private Animation dialogue;
    [Tooltip("If dialogue is connected to cutscene")]
    [SerializeField] private Animation cutscene;
    [Tooltip("If dialogue shows not a start, set delay (Example: at 5 second activated dialogue)")]
    [SerializeField] private float cutsceneDelay = 0f;
    [Tooltip("Steps for this dialogue")]
    [SerializeField] private float[] stepTimes;
    [Tooltip("Skip step")]
    [SerializeField] private Button skipStepInDialogue;
    [Tooltip("Connect to 'DelayStartTimer' class")]
    [SerializeField] private DelayStartTimer timer;
    [Header("__ Cutscene second camera system __")]
    [Tooltip("Don't add this component, if you don't use it, or you will get errors.")]
    [SerializeField] private CutsceneSecondCameraSystem secondCameraSystem;
    [Tooltip("Add cutscene camera id's when you use it. This id points are connected to 'stepTimes'.")]
    [SerializeField] private int[] cameraPointId;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip whoosh;
    [Space(10)]
    [Header("__ Voices: Default __")]
    [SerializeField] private AudioClip defaultClip;
    //[Header("__ Voices: English __")]
    //[SerializeField] private AudioClip[] engClip;
    [Header("__ Voices: Russian __")]
    [SerializeField] private AudioClip[] rusClip;

    private int _usingLanguage;
    private int _usingVoiceId;
    private int _stepId = 0;
    private bool _changeCutscene;
    private bool _isSaying = false;

    private bool _Debug = false;

    private void Awake() { _usingLanguage = KeyManager.Get_Bool_Key("Language"); }

    private void Start() { if (skipStepInDialogue != null) { skipStepInDialogue.onClick.AddListener(SkipDialoguePart); }}

    private void OnEnable() { _stepId = 0; }

    private void OnDestroy() { if (skipStepInDialogue != null) { skipStepInDialogue.onClick.RemoveListener(SkipDialoguePart); }}

    public void SayText(int id)
    {
        if (_isSaying) { return; }
        _stepId = id + 1;
        src.PlayOneShot(whoosh);
        _usingVoiceId = id;
        _isSaying = true;
        Invoke(nameof(_SayText), 0.5f);
    }

    private void SkipDialoguePart()
    {
        if (dialogue == null) { return; }

        if (src != null) { src.Stop(); }

        if (_stepId >= stepTimes.Length) { timer.Cancel(); return; }
        else { timer.CalculateNewTimer(stepTimes[_stepId]); }

        AnimationState dialogueState = dialogue[dialogue.clip.name];
        dialogueState.time = stepTimes[_stepId];
        dialogue.Play();

        if (cutscene != null) { _changeCutscene = true; }
        else { _changeCutscene = false; }

        AnimationState cutsceneState;
        if (_changeCutscene == true)
        {
            cutsceneState = cutscene[cutscene.clip.name];
            cutsceneState.time = stepTimes[_stepId] + cutsceneDelay;
            cutscene.Play();
        }

        if (secondCameraSystem != null) { secondCameraSystem.RecalculateCameraWaypoint(cameraPointId[_stepId]); }
    }

    private void _SayText()
    {
        if (_Debug) { src.PlayOneShot(defaultClip); _usingVoiceId = -1; _isSaying = false; return; }

        switch (_usingLanguage)
        {
            case 0: src.PlayOneShot(defaultClip); break;
            case 1: src.PlayOneShot(rusClip[_usingVoiceId]); break;
            default: src.PlayOneShot(defaultClip); break;
        }

        _isSaying = false;
        _usingVoiceId = -1;
    }
}

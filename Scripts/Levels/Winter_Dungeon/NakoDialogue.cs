using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class NakoDialogue : MonoBehaviour
{
    [Header("__ Nako __")]
    [SerializeField] private GameObject Nako;
    [SerializeField] private Button[] hideNako;
    [Header("__ UI __")]
    [SerializeField] private Button startSkip;
    [SerializeField] private Button startDialogue;
    [SerializeField] private GameObject startDialogueWindow;
    [SerializeField] private float duration = 20f;
    [SerializeField] private RectTransform selectWords;
    [Header("__ Dialogue First __")]
    [SerializeField] private Button goodSkip;
    [SerializeField] private GameObject goodEnding;
    [SerializeField] private Button goodEndingButton;
    [SerializeField] private float delaySpawn = 20f;
    [SerializeField] private GameObject artifact;
    [SerializeField] private Transform artifactSpawn;
    [Header("__ Dialogue Second __")]
    [SerializeField] private Animation animDeath;
    [SerializeField] private string animDeathName;
    [SerializeField] private GameObject badEnding;
    [SerializeField] private Button badEndingButton;
    [Header("__ Dialogue Third __")]
    [SerializeField] private Animation animKilling;
    [SerializeField] private string animKillingName;
    [SerializeField] private float delayKill = 10f;
    [SerializeField] private UnityEvent KillingPlayer;
    [SerializeField] private GameObject worstEnding;
    [SerializeField] private Button worstEndingButton;
    [Header("__ Events __")]
    [SerializeField] private Button enterSecret;
    [SerializeField] private Button exitSecret;
    [SerializeField] private UnityEvent PauseTime;
    [SerializeField] private UnityEvent PlayTime;

    private Vector2 shown = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);
    private string key = "wasDialogueWithNako";
    private bool _isTalked = false;

    private void Start()
    {
        startDialogue.onClick.AddListener(StartDialogue);
        goodEndingButton.onClick.AddListener(GoodEnding);
        badEndingButton.onClick.AddListener(BadEnding);
        worstEndingButton.onClick.AddListener(WorstEnding);

        startSkip.onClick.AddListener(StartSkip);
        goodSkip.onClick.AddListener(GoodSkip);

        enterSecret.onClick.AddListener(PauseTimer);
        exitSecret.onClick.AddListener(PlayTimer);

        foreach (Button btn in hideNako) { btn.onClick.AddListener(HideNako); }

        int was = KeyManager.Get_Bool_Key(key);
        if (was == 1) { Nako.SetActive(false); }
    }

    private void OnDestroy()
    {
        startDialogue.onClick.RemoveListener(StartDialogue);
        goodEndingButton.onClick.RemoveListener(GoodEnding);

        startSkip.onClick.RemoveListener(StartSkip);
        goodSkip.onClick.RemoveListener(GoodSkip);

        enterSecret.onClick.RemoveListener(PauseTimer);
        exitSecret.onClick.RemoveListener(PlayTimer);

        foreach (Button btn in hideNako) { btn.onClick.RemoveListener(HideNako); }
    }

    private void StartDialogue()
    {
        _isTalked = true;
        startDialogueWindow.SetActive(true);
        Invoke(nameof(ShowEndings), duration);
    }

    private void ShowEndings() { selectWords.DOAnchorPos(shown, 0.5f); }

    private void GoodEnding()
    {
        selectWords.DOAnchorPos(hidden, 0.5f);
        goodEnding.SetActive(true);
        Invoke(nameof(GivingArtifact), delaySpawn);
        KeyManager.Set_Bool_Key(key, 1);
    }

    private void GivingArtifact() { Instantiate(artifact, artifactSpawn.position, artifactSpawn.rotation); }

    private void BadEnding()
    {
        selectWords.DOAnchorPos(hidden, 0.5f);
        badEnding.SetActive(true);
        animDeath.Play(animDeathName);
        KeyManager.Set_Bool_Key(key, 1);
    }

    private void WorstEnding()
    {
        selectWords.DOAnchorPos(hidden, 0.5f);
        worstEnding.SetActive(true);
        animKilling.Play(animKillingName);
        Invoke(nameof(PlayerKilling), delayKill);
        KeyManager.Set_Bool_Key(key, 1);
    }

    private void PlayerKilling() { KillingPlayer.Invoke(); }

    private void HideNako() { if (!_isTalked) { return; } Nako.SetActive(false); }

    private void PlayTimer() { Invoke(nameof(_PlayTimer), 0.5f); }
    private void PauseTimer() { Invoke(nameof(_PauseTimer), 0.5f); }

    private void _PlayTimer() { PlayTime.Invoke(); }
    private void _PauseTimer() { PauseTime.Invoke(); }

    private void StartSkip()
    {
        CancelInvoke(nameof(ShowEndings));
        ShowEndings();
    }

    private void GoodSkip()
    {
        CancelInvoke(nameof(GivingArtifact));
        GivingArtifact();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TutorialSceneGuide : MonoBehaviour
{
    [Header("__ Dialogues __")]
    [SerializeField] private GameObject startDialogue;
    [SerializeField] private GameObject beerDialogue;
    [SerializeField] private GameObject endDialogue;
    [Header("__ Help __")]
    [SerializeField] private GameObject spyKill;
    [SerializeField] private GameObject buyBeer;
    [SerializeField] private GameObject giveBeer;
    [SerializeField] private Animation spyKillAnim;
    [SerializeField] private Animation buyBeerAnim;
    [SerializeField] private Animation giveBeerAnim;
    [Header("__ Cursor help __")]
    [Tooltip("For this scene must be 4, but for other scenery your cursorHelp.Lenght must be same as _action.Length.")]
    [SerializeField] private GameObject[] cursorHelp = new GameObject[4]; // 4 штуки должно быть в итоге
    [Header("__ Special __")]
    [SerializeField] private GameObject generalsBeer;
    [Header("__ Loader __")]
    [SerializeField] private GameObject loader;
    [SerializeField] private int nextSceneId = 16;

    private bool _boughtBeer = false;
    private bool _blockedCursorGuide = true;
    private UnityAction[] _action = new UnityAction[4];

    #region Dialogues

    private void Start() { startDialogue.SetActive(true); }
    public void ShowBeerDialogue() { beerDialogue.SetActive(true); }
    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player") && _boughtBeer) { endDialogue.SetActive(true); generalsBeer.SetActive(true); } }

    #endregion

    public void EndLastDialogue() { loader.SetActive(true); Invoke(nameof(LoadScene), 0.5f); }
    private void LoadScene() { LoadLevel.LoadLevelById(nextSceneId); }

    #region Help

    public void ShowHelp_SpyKill()
    {
        spyKill.SetActive(true);
        spyKillAnim.Play();
    }

    public void ShowHelp_BuyBeer()
    {
        buyBeer.SetActive(true);
        buyBeerAnim.Play();
    }

    public void ShowHelp_GiveBeer()
    {
        giveBeer.SetActive(true);
        giveBeerAnim.Play();
        _boughtBeer = true;
    }

    #endregion

    #region Cursor helper

    public void EndedHelp() { _blockedCursorGuide = true; }
    public void StartedHelp() { _blockedCursorGuide = false; }

    public void GetCursorHelp(int helpId)
    {
        if (_blockedCursorGuide) { return; }
        foreach (GameObject helpCursor in cursorHelp) { helpCursor.SetActive(false); }
        if (helpId >= 0 && helpId <= 3) { cursorHelp[helpId].SetActive(true); }
    }

    #endregion
}

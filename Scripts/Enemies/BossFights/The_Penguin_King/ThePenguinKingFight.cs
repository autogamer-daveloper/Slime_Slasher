using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ThePenguinKingFight : MonoBehaviour
{
    [Header("__ Cameras __")]
    [SerializeField] private GameObject cutsceneCamera;
    [SerializeField] private GameObject gameplayCamera;
    [Header("__ UI __")]
    [SerializeField] private GameObject Dialogue;
    [SerializeField] private float showDialogue = 7f;
    [SerializeField] private RectTransform ui;
    [SerializeField] private TMP_Text stageTextUI;
    [SerializeField] private Button grenadeButton;
    [Header("__ Settings __")]
    [SerializeField] private EndGameCutsceneLoad endGame;
    [SerializeField] private float startGameplay = 55f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform penguin;
    [SerializeField] private Transform grenade;
    [SerializeField] private GameObject grenadeObject;
    [Header("__ Penguin Animations __")]
    [SerializeField] private GameObject[] penguinTextures;
    [SerializeField] private Animation[] penguinAnimations;
    [Header("__ Spawner __")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int[] enemiesCount = { 5, 5, 1 };
    [SerializeField] private string[] stageText = { "Stage 1", "Stage 2", "Stage 3", "FINISH HIM!" };
    [SerializeField] private Transform spawnPoint;
    [Header("__ Traps __")]
    [SerializeField] private Animation[] trap;

    private Vector2 hidden = new Vector2(5000, 5000);
    private Vector2 shown = new Vector2(0, 0);
    private Vector2 penguinPos = new Vector2(0, 0);

    private int _stage;
    private int _spawned;
    private int _killed;

    private void Start()
    {
        ShowPenguinAnimation(0);
        ShowToStageText("");
        ui.DOSizeDelta(hidden, 0.1f);
        penguinPos = new Vector2(penguin.position.x, penguin.position.y);
        grenadeObject.SetActive(false);

        grenadeButton.onClick.AddListener(DropGrenade);

        Invoke(nameof(ShowDialogue), showDialogue);
        Invoke(nameof(StartGame), startGameplay);
        Invoke(nameof(ShowInterface), startGameplay - 5f);
    }

    private void OnDestroy() { grenadeButton.onClick.AddListener(DropGrenade); }

    private void ShowDialogue() { Dialogue.SetActive(true); }
    private void ShowToStageText(string txt) { stageTextUI.text = txt; }
    private void ShowInterface() { ui.DOSizeDelta(shown, 5f); }

    private void StartGame()
    {
        cutsceneCamera.SetActive(false);
        gameplayCamera.SetActive(true);
        _stage = 0;
        _killed = 0;
        ShowToStageText(stageText[_stage]);
        Invoke(nameof(Spawn), 0.5f);
        TrapActivate();
    }

    private void Spawn()
    {
        if (_stage >= enemies.Length) { CancelInvoke(nameof(TrapActivate)); return; }

        Instantiate(enemies[_stage], spawnPoint.position, spawnPoint.rotation);
        _spawned++;
        if (_spawned >= enemiesCount[_stage]) { return; }
        else { Invoke(nameof(Spawn), 3f); }
    }

    public void KilledPenguin()
    {
        _killed++;
        if (_killed >= enemiesCount[_stage])
        {
            _spawned = 0;
            _killed = 0;
            _stage++;
            ShowToStageText(stageText[_stage]);
            Invoke(nameof(Spawn), 0.5f);
        }
    }

    private void TrapActivate()
    {
        if (_stage >= enemies.Length) return;
        int random = Random.Range(0, trap.Length);
        trap[random].Stop();
        trap[random].Play();
        ShowPenguinAnimation(1);

        Invoke(nameof(ShowIdlePenguin), 0.5f);
        Invoke(nameof(TrapActivate), 1.1f);
    }

    private void ShowIdlePenguin() { ShowPenguinAnimation(0); }
    private void ShowExplodedPenguin() { ShowPenguinAnimation(2); }

    private void ShowPenguinAnimation(int id)
    {
        foreach (GameObject textures in penguinTextures) { textures.SetActive(false); }
        penguinTextures[id].SetActive(true);
        penguinAnimations[id].Play();
    }

    private void DropGrenade()
    {
        Vector2 grenadeStartPos = new Vector2(player.position.x, player.position.y);
        grenade.position = grenadeStartPos;
        grenadeObject.SetActive(true);
        grenade.DOMove(penguinPos, 1.5f).OnComplete(() => { grenadeObject.SetActive(false); });
        Invoke(nameof(ShowExplodedPenguin), 1.5f);
        Invoke(nameof(PlayerKill), 2f);
    }

    public void PlayerDead() { endGame.EndGame(false); }
    private void PlayerKill() { endGame.EndGame(true); }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class RyanSamuraiBossFight : MonoBehaviour
{
    [Header("__ Health __")]
    [SerializeField] private EnemyStats stats;
    [Header("__ Animations __")]
    [SerializeField] private GameObject[] animationObj; // 0 - 7. 0 sit, 1 idle!, 2 idle, 3 attackSimple, 4 Immortal, 5 pre-dying, 6-dying, 7-self-dying
    [SerializeField] private Animation[] animations; // 0 - 7
    [Header("__ Boss defeated key __")]
    [SerializeField] private string bossKey = "isRyanSamuraiDefeated";
    [Header("__ UI __")]
    [SerializeField] private Button startSkip;
    [SerializeField] private Button startFight;
    [SerializeField] private GameObject startDialogue;
    [SerializeField] private RectTransform fightOrNot;
    [SerializeField] private GameObject fightOrNotObj;
    [SerializeField] private Button fight;
    [SerializeField] private Button notFight;
    [SerializeField] private GameObject startFightD;
    [SerializeField] private GameObject endFightD;
    [SerializeField] private GameObject notStartFightD;
    [SerializeField] private GameObject help;
    [SerializeField] private Animation helpAnim;
    [Header("__ Other __")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform nextToPlayer;
    [SerializeField] private GameObject samuraiSlashLeft;
    [SerializeField] private GameObject samuraiSlashRight;
    [SerializeField] private GameObject coil01;
    [SerializeField] private GameObject coil02;
    [SerializeField] private UnityEvent afterKilling;
    [SerializeField] private UnityEvent afterKillingSuicide;
    [SerializeField] private UnityEvent onLosed;
    [SerializeField] private GameObject playerVisible;
    [SerializeField] private GameObject[] activateInFight;
    [SerializeField] private GameObject[] deactivateInFight;
    [SerializeField] private GameObject[] deactivateAfterFight;
    [SerializeField] private Transform boss;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Animation samuraiEye;
    [SerializeField] private UnityEvent onStartedFight;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip magic;
    [SerializeField] private AudioClip slash;
    [SerializeField] private AudioClip fear;

    private int _isSamuraiDefeated = 0;
    private bool _isSecondPhase = false;
    private bool _damageBlocked;
    private bool _isNextRight = true;
    private bool _wasFightStarted = false;

    //Timings
    private float _ImmortalTime = 0.75f;
    private float _beforeFirstDialogue = 0.5f;
    private float _firstDialogueDuration = 55.5f;
    private float _beforeFight = 11.5f;
    private float _beforeSelfKill = 10.5f;
    private float _afterSpawn = 4f;
    private float _betweenCoilSpawns = 0.15f;
    private float _afterCoilSpawns = 1.25f;

    private int _coilCount = 0;
    private int _slashCount = 0;

    private Vector2 _shown = new Vector2(0, 0);
    private Vector2 _hidden = new Vector2(0, -2000);

    private List<GameObject> _slashes = new List<GameObject>();

    #region MAIN

    private void Start()
    {
        playerVisible.SetActive(false);
        stats.BlockDamage();
        _isSamuraiDefeated = KeyManager.Get_Bool_Key(bossKey);

        if (_isSamuraiDefeated == 1) { showAnimation(5); afterKilling.Invoke(); }
        else { showAnimation(0); }

        startFight.onClick.AddListener(PreStartFight);
        fight.onClick.AddListener(StartFight);
        notFight.onClick.AddListener(NotStartFight);
        startSkip.onClick.AddListener(StartSkip);
    }

    private void OnDestroy()
    {
        startFight.onClick.RemoveListener(PreStartFight);
        fight.onClick.RemoveListener(StartFight);
        notFight.onClick.RemoveListener(NotStartFight);
        startSkip.onClick.RemoveListener(StartSkip);
    }

    private void showAnimation(int id)
    {
        if (id == 4) { Invoke(nameof(TeleportToPlayer), 0.5f); }

        foreach (GameObject obj in animationObj) { if (obj != null) obj.SetActive(false); }

        animationObj[id].SetActive(true);
        animations[id].Play();
    }

    private void TeleportToPlayer() { boss.position = nextToPlayer.position; }

    //internal string GetBossKey() { return bossKey; }

    #endregion

    #region DIALOGUES

    private void StartSkip()
    {
        CancelInvoke(nameof(ShowSelecting));
        ShowSelecting();
    }

    private void PreStartFight()
    {
        showAnimation(1);
        Invoke(nameof(ShowFirstDialogue), _beforeFirstDialogue);
        healthBar.SetActive(true);
    }

    private void ShowFirstDialogue()
    {
        startDialogue.SetActive(true);
        Invoke(nameof(ShowSelecting), _firstDialogueDuration);
    }

    private void ShowSelecting() { fightOrNotObj.SetActive(true); fightOrNot.DOAnchorPos(_shown, 0.5f); }
    private void HideSelecting() { fightOrNot.DOAnchorPos(_hidden, 0.5f).OnComplete(() => { fightOrNotObj.SetActive(false); }); }

    private void StartFight()
    {
        HideSelecting();
        onStartedFight.Invoke();
        startFightD.SetActive(true);
        Invoke(nameof(Fight), _beforeFight);
    }

    private void NotStartFight()
    {
        HideSelecting();
        notStartFightD.SetActive(true);
        Invoke(nameof(Suicide), _beforeSelfKill);
    }

    private void Suicide()
    {
        showAnimation(6);
        healthBar.SetActive(false);
        stats.BlockDamage();
        Invoke(nameof(OnBossDefeated), 0.5f);
        KeyManager.Set_Bool_Key(bossKey, 1);
        afterKillingSuicide.Invoke();
    }

    #endregion

    #region FIGHT

    private void Fight()
    {
        playerVisible.SetActive(true);
        _wasFightStarted = true;
        showAnimation(2);
        stats.UnlockDamage();
        Invoke(nameof(SpawnCoils), 0.5f);

        foreach (GameObject obj in activateInFight) { obj.SetActive(true); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(false); }
    }

    private void SpawnCoils()
    {
        src.PlayOneShot(magic);
        if (!_isSecondPhase)
        {
            Instantiate(coil01, player.position, player.rotation);
            if (_coilCount <= 4) { Invoke(nameof(SpawnCoils), _betweenCoilSpawns); }
            else { Invoke(nameof(SpawnEnemy), _afterCoilSpawns); }
        }
        else
        {
            Instantiate(coil02, player.position, player.rotation);
            if (_coilCount <= 9) { Invoke(nameof(SpawnCoils), _betweenCoilSpawns); }
            else { Invoke(nameof(SpawnEnemy), _afterCoilSpawns); }
            
        }
        _coilCount++;
        _slashCount = 0;
    }

    private void SpawnEnemy()
    {
        Invoke(nameof(_SpawnEnemy), 0.5f);
    }

    private void _SpawnEnemy()
    {
        CheckSlashes();
        GameObject slashObj;
        src.PlayOneShot(slash);
        if (_slashCount == 0) { samuraiEye.Stop(); samuraiEye.Play(); src.PlayOneShot(fear); }
        Debug.LogWarning("SPAWN RYANS ILLUSIONS");
        if (!_isSecondPhase)
        {
            if (_isNextRight) { slashObj = Instantiate(samuraiSlashRight, player.position, player.rotation); }
            else { slashObj = Instantiate(samuraiSlashLeft, player.position, player.rotation); }
            if (_slashCount <= 2) { Invoke(nameof(_SpawnEnemy), 0.5f); }
            else { Invoke(nameof(SpawnCoils), _afterSpawn); }
        }
        else
        {
            if (_isNextRight) { slashObj = Instantiate(samuraiSlashRight, player.position, player.rotation); }
            else { slashObj = Instantiate(samuraiSlashLeft, player.position, player.rotation); }
            if (_slashCount <= 4) { Invoke(nameof(_SpawnEnemy), 0.35f); }
            else { Invoke(nameof(SpawnCoils), _afterSpawn); }
        }
        _isNextRight = !_isNextRight;
        _slashCount++;
        _slashes.Add(slashObj);
        _coilCount = 0;
    }

    public void SetSecondPhase()
    {
        CancelInvoke(nameof(SpawnCoils));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        showAnimation(4);
        samuraiEye.Stop();
        samuraiEye.Play();
        src.PlayOneShot(fear);
        _isSecondPhase = true;
        Invoke(nameof(ShowIdle), _ImmortalTime);
        Invoke(nameof(Fight), 0.5f);
    }

    public void AttackPlayer()
    {
        if (!_wasFightStarted) return;
        showAnimation(3);
        Invoke(nameof(ShowIdle), 0.5f);
    }

    #endregion

    #region AFTER FIGHT

    public void PlayerDead()
    {
        stats.BlockDamage();
        CancelInvoke(nameof(SpawnCoils));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        foreach (GameObject obj in activateInFight) { obj.SetActive(false); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(true); }
        foreach (GameObject obj in deactivateAfterFight) { obj.SetActive(false); }
        boss.position = startPos.position;
        playerVisible.SetActive(false);
        showAnimation(0);
    }

    public void Killed()
    {
        DeleteSlashes();
        healthBar.SetActive(false);
        CancelInvoke(nameof(SpawnCoils));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        stats.BlockDamage();
        endFightD.SetActive(true);
        _wasFightStarted = false;
        playerVisible.SetActive(false);
        Invoke(nameof(OnBossDefeated), 2.5f);
        KeyManager.Set_Bool_Key(bossKey, 1);
        afterKilling.Invoke();
        showAnimation(5);
    }

    private void OnBossDefeated()
    {
        foreach (GameObject obj in activateInFight) { obj.SetActive(false); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(true); }
        foreach (GameObject obj in deactivateAfterFight) { obj.SetActive(false); }
        Help();
    }

    public void Help() { help.SetActive(true); helpAnim.Play(); }

    private void ShowIdle() { if (_wasFightStarted) showAnimation(2); }

    #endregion

    private void CheckSlashes() { _slashes.RemoveAll(item => item == null); }

    private void DeleteSlashes()
    {
        foreach (GameObject obj in _slashes) { if (obj != null) { Destroy(obj); }}
        _slashes.Clear();
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DenseTreeLogic : MonoBehaviour
{
    [Header("__ Health __")]
    [SerializeField] private EnemyStats stats;
    [Header("__ Animations __")]
    [SerializeField] private GameObject[] animationObj; // 0 - 6. 0 font, 1 morpg, 2 idle, 3 attack1, 4 attack2, 5 dying, 6 die
    [SerializeField] private Animation[] animations; // 0 - 6
    [Header("__ Boss defeated key __")]
    [SerializeField] private string bossKey = "isDenseTreeDefeated";
    [Header("__ UI __")]
    [SerializeField] private Button startFight;
    [SerializeField] private GameObject startDialogue;
    [SerializeField] private GameObject finalDialogue;
    [SerializeField] private Animation flashbang;
    [Header("__ Other __")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] enemySpawners;
    [SerializeField] private GameObject druid01;
    [SerializeField] private GameObject druid02;
    [SerializeField] private GameObject wine01;
    [SerializeField] private GameObject wine02;
    [SerializeField] private UnityEvent spawned;
    [SerializeField] private UnityEvent afterKilling;
    [SerializeField] private GameObject wines;
    [SerializeField] private GameObject[] activateInFight;
    [SerializeField] private GameObject[] deactivateInFight;
    [SerializeField] private GameObject healthBar;
    [Header("__ Audio Setting __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip winesClip;
    [SerializeField] private AudioClip whoosh;
    [SerializeField] private AudioClip magic;

    private int _isDenseTreeDefeated = 0;
    private bool _isSecondPhase = false;
    private bool _damageBlocked;

    //Timings
    private float _attackTime = 0.2f;
    private float _beforeFirstDialogue = 0.5f;
    private float _FinalDialogue = 22f;
    private float _beforeFight = 6f;
    private float _afterSpawn = 4f;
    private float _betweenWineSpawns = 0.33f;
    private float _afterWineSpawns = 2f;

    private int wineCount = 0;

    private List<GameObject> _spawnedMinions = new List<GameObject>();

    private void Start()
    {
        stats.BlockDamage();
        _isDenseTreeDefeated = KeyManager.Get_Bool_Key(bossKey);

        if (_isDenseTreeDefeated == 1)
        {
            showAnimation(6);
            wines.SetActive(true);
        }
        else
        {
            showAnimation(0);
        }

        startFight.onClick.AddListener(StartFight);
    }

    private void OnDestroy()
    {
        startFight.onClick.RemoveListener(StartFight);
    }

    private void showAnimation(int id)
    {
        foreach (GameObject obj in animationObj)
        {
            obj.SetActive(false);
        }

        animationObj[id].SetActive(true);
        animations[id].Play();
    }

    private void StartFight()
    {
        showAnimation(1);
        spawned.Invoke();
        Invoke(nameof(ShowFirstDialogue), _beforeFirstDialogue);
        healthBar.SetActive(true);

        foreach (GameObject obj in activateInFight) { obj.SetActive(true); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(false); }
    }

    private void ShowFirstDialogue()
    {
        showAnimation(2);
        startDialogue.SetActive(true);
        Invoke(nameof(Fight), _beforeFight);
    }

    private void Fight()
    {
        stats.UnlockDamage();
        Invoke(nameof(SpawnEnemy), 0.5f);
    }

    private void SpawnWines()
    {
        src.PlayOneShot(magic);
        if (!_isSecondPhase)
        {
            showAnimation(3);
            Invoke(nameof(ShowIdle), _attackTime);
            Instantiate(wine01, player.position, player.rotation);
            if (wineCount <= 1)
            {
                Invoke(nameof(SpawnWines), _betweenWineSpawns);
            }
            else
            {
                Invoke(nameof(SpawnEnemy), _afterWineSpawns);
            }
            wineCount++;
        }
        else
        {
            showAnimation(4);
            Invoke(nameof(ShowIdle), _attackTime);
            Instantiate(wine02, player.position, player.rotation);
            if (wineCount <= 3)
            {
                Invoke(nameof(SpawnWines), _betweenWineSpawns);
            }
            else
            {
                Invoke(nameof(SpawnEnemy), _afterWineSpawns);
            }
            wineCount++;
        }
    }

    private void SpawnEnemy()
    {
        flashbang.Play();
        src.PlayOneShot(whoosh);
        Invoke(nameof(_SpawnEnemy), 0.5f);
    }

    private void _SpawnEnemy()
    {
        Debug.LogWarning("SPAWN DRUIDS");
        wineCount = 0;
        Invoke(nameof(SpawnWines), _afterSpawn);

        _spawnedMinions.RemoveAll(item => item == null);
        if (_spawnedMinions.Count > 0) { return; }
        foreach (Transform point in enemySpawners)
        {
            if (!_isSecondPhase)
            {
                GameObject spawned;

                showAnimation(3);
                Invoke(nameof(ShowIdle), _attackTime);
                spawned = Instantiate(druid01, point.position, point.rotation);

                _spawnedMinions.Add(spawned);
            }
            else
            {
                GameObject spawned;

                showAnimation(4);
                Invoke(nameof(ShowIdle), _attackTime);
                spawned = Instantiate(druid02, point.position, point.rotation);

                _spawnedMinions.Add(spawned);
            }
        }
    }

    public void PlayerDead()
    {
        stats.BlockDamage();
        CancelInvoke(nameof(SpawnWines));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        foreach (GameObject obj in activateInFight) { obj.SetActive(false); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(true); }

        showAnimation(0);
    }

    public void Killed()
    {
        stats.BlockDamage();
        CancelInvoke(nameof(SpawnWines));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        foreach (GameObject obj in activateInFight) { obj.SetActive(false); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(true); }

        showAnimation(5);
        finalDialogue.SetActive(true);
        KeyManager.Set_Bool_Key(bossKey, 1);
        afterKilling.Invoke();
        Invoke(nameof(ShowDeadTree), _FinalDialogue);
    }

    private void ShowDeadTree()
    {
        showAnimation(6);
    }

    private void ShowIdle()
    {
        showAnimation(2);
    }

    public void SetSecondPhase()
    {
        CancelInvoke(nameof(SpawnWines));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        showAnimation(4);
        _isSecondPhase = true;
        wines.SetActive(true);
        src.PlayOneShot(winesClip);
        Invoke(nameof(ShowIdle), _attackTime);
        Invoke(nameof(Fight), 0.5f);
    }

    public void CheckMinions()
    {
        _spawnedMinions.RemoveAll(item => item == null);

        bool minionsAlive = _spawnedMinions.Count > 0;

        if (minionsAlive == _damageBlocked)
            return;

        _damageBlocked = minionsAlive;

        if (minionsAlive)
            MinionsAlive();
        else
            MinionsDead();
    }

    private void MinionsAlive() { stats.BlockDamage(); }

    private void MinionsDead() { stats.UnlockDamage(); }

    internal string GetBossKey() { return bossKey; }
}

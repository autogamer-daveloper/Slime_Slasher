using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkeletonLordBossFight : MonoBehaviour
{
    [Header("__ Health __")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameObject playerVision;
    [Header("__ Animations __")]
    [SerializeField] private GameObject[] animationObj; // 0 - 6. 0 font, 1 morpg, 2 idle, 3 attack1, 4 attack2, 5 dying
    [SerializeField] private Animation[] animations; // 0 - 6
    [Header("__ Boss defeated key __")]
    [SerializeField] private string bossKey = "isSkeletonLordDefeated";
    [Header("__ UI __")]
    [SerializeField] private Button startFight;
    [SerializeField] private GameObject startDialogue;
    [Header("__ Other __")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] enemySpawners;
    [SerializeField] private GameObject skeleton01;
    [SerializeField] private GameObject skeleton02;
    [SerializeField] private GameObject coil01;
    [SerializeField] private GameObject coil02;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private UnityEvent spawned;
    [SerializeField] private UnityEvent afterKilling;
    [SerializeField] private GameObject[] activateInFight;
    [SerializeField] private GameObject[] deactivateInFight;
    [SerializeField] private GameObject[] deactivateAfterFight;
    [SerializeField] private Transform boss;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private SkeletonUniqueAttack uniqueAttack;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip magicAttack;
    [SerializeField] private AudioClip circleAttack;

    private int _isSkeletonLordDefeated = 0;
    private bool _isSecondPhase = false;
    private bool _damageBlocked;

    //Timings
    private float _attackTime = 0.2f;
    private float _beforeFirstDialogue = 0.5f;
    private float _beforeFight = 6f;
    private float _afterSpawn = 4f;
    private float _betweenCoilSpawns = 0.5f;
    private float _afterCoilSpawns = 7f;

    private int coilCount = 0;

    private List<GameObject> _spawnedMinions = new List<GameObject>();

    private void Start()
    {
        stats.BlockDamage();
        _isSkeletonLordDefeated = KeyManager.Get_Bool_Key(bossKey);
        playerVision.SetActive(false);

        if (_isSkeletonLordDefeated == 1)
        {
            showAnimation(5);
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

        if(animationObj[id] != null) animationObj[id].SetActive(true);
        if(animations[id] != null) animations[id].Play();
    }

    private void StartFight()
    {
        showAnimation(1);
        spawned.Invoke();
        Invoke(nameof(ShowFirstDialogue), _beforeFirstDialogue);
        healthBar.SetActive(true);
        playerVision.SetActive(true);

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

    private void SpawnCoils()
    {
        uniqueAttack.Attack();
        src.PlayOneShot(circleAttack);
        _SpawnCoils();
    }

    private void _SpawnCoils()
    {
        if (!_isSecondPhase)
        {
            showAnimation(3);
            Invoke(nameof(ShowIdle), _attackTime);
            Instantiate(coil01, player.position, player.rotation);
            src.PlayOneShot(magicAttack);
            if (coilCount <= 1)
            {
                Invoke(nameof(_SpawnCoils), _betweenCoilSpawns);
            }
            else
            {
                Invoke(nameof(SpawnEnemy), _afterCoilSpawns);
            }
            coilCount++;
        }
        else
        {
            showAnimation(4);
            Invoke(nameof(ShowIdle), _attackTime);
            Instantiate(coil02, player.position, player.rotation);
            src.PlayOneShot(magicAttack);
            if (coilCount <= 3)
            {
                Invoke(nameof(_SpawnCoils), _betweenCoilSpawns);
            }
            else
            {
                Invoke(nameof(SpawnEnemy), _afterCoilSpawns);
            }
            coilCount++;
        }
    }

    private void SpawnEnemy()
    {
        Invoke(nameof(_SpawnEnemy), 0.5f);
    }

    private void _SpawnEnemy()
    {
        src.PlayOneShot(circleAttack);
        Debug.LogWarning("SPAWN SKELETONS");
        coilCount = 0;
        Invoke(nameof(SpawnCoils), _afterSpawn);

        _spawnedMinions.RemoveAll(item => item == null);
        //if (_spawnedMinions.Count > 0) { return; }
        foreach (Transform point in enemySpawners)
        {
            Instantiate(spawnEffect, point.position, point.rotation);
            if (!_isSecondPhase)
            {
                GameObject spawned;

                showAnimation(3);
                Invoke(nameof(ShowIdle), _attackTime);
                spawned = Instantiate(skeleton01, point.position, point.rotation);

                _spawnedMinions.Add(spawned);
            }
            else
            {
                GameObject spawned;

                showAnimation(4);
                Invoke(nameof(ShowIdle), _attackTime);
                spawned = Instantiate(skeleton02, point.position, point.rotation);

                _spawnedMinions.Add(spawned);
            }
        }
    }

    public void PlayerDead()
    {
        playerVision.SetActive(false);
        stats.BlockDamage();
        CancelInvoke(nameof(SpawnCoils));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        foreach (GameObject obj in activateInFight) { obj.SetActive(false); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(true); }
        foreach (GameObject obj in deactivateAfterFight) { obj.SetActive(false); }
        boss.position = startPos.position;
        showAnimation(0);
    }

    public void Killed()
    {
        playerVision.SetActive(false);
        stats.BlockDamage();
        CancelInvoke(nameof(SpawnCoils));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));

        Invoke(nameof(OnBossDefeated), 0.5f);
        showAnimation(5);
        KeyManager.Set_Bool_Key(bossKey, 1);
        afterKilling.Invoke();
    }

    private void OnBossDefeated()
    {
        foreach (GameObject obj in activateInFight) { obj.SetActive(false); }
        foreach (GameObject obj in deactivateInFight) { obj.SetActive(true); }
        foreach (GameObject obj in deactivateAfterFight) { obj.SetActive(false); }
    }

    private void ShowIdle()
    {
        showAnimation(2);
        playerVision.SetActive(true);
    }

    public void SetSecondPhase()
    {
        playerVision.SetActive(false);
        CancelInvoke(nameof(SpawnCoils));
        CancelInvoke(nameof(SpawnEnemy));
        CancelInvoke(nameof(_SpawnEnemy));
        showAnimation(4);
        _isSecondPhase = true;
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

    private void MinionsAlive()
    {
        stats.BlockDamage();
    }

    private void MinionsDead()
    {
        stats.UnlockDamage();
    }

    internal string GetBossKey()
    {
        return bossKey;
    }
}
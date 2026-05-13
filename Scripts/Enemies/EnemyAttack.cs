using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack - Main")]
    [SerializeField] private float speed;
    [SerializeField] private bool boss = false;
    [SerializeField] private float distantion = 10f;

    [Header("Attack - Melee")]
    [SerializeField] private Animation meeleAnim;
    [SerializeField] private bool isHaveCustomPunch = false;
    [SerializeField] private UnityEvent CustomPunch;

    [Header("Attack - Range")]
    [SerializeField] private Transform aim;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bullet;

    [Header("__ Audio __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip sound;

    [Header("__ Rotation __")]
    [Tooltip("Градусы, чтобы подогнать ориентацию спрайта (например, если спрайт 'смотрит' вверх, поставьте 90).")]
    [SerializeField] private float rotationOffset = 0f;
    [Tooltip("Скорость плавного поворота (градусы/сек). Если 0 — поворот будет мгновенным.")]
    [SerializeField] private float rotationSpeed = 360f;

    private bool _isRange = false;
    private bool _isAttacking = false;

    private bool _isBlocked = false;
    private bool _isBossLock = false;
    private bool _visionBlock = false;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Checking), 0.1f, 0.1f);

        if (boss)
        {
            _isBossLock = true;
            Invoke(nameof(UnlockAttacks), 7.5f);
        }
    }

    private void OnDestroy() { CancelInvoke(nameof(Checking)); }
    private void OnDisable() { CancelInvoke(nameof(Checking)); }

    private void UnlockAttacks() { _isBossLock = false; }

    internal void isRangeEnemy(bool answer) { _isRange = answer; }

    private void Checking()
    {
        if (_isRange && aim != null && bulletSpawn != null)
        {
            Vector3 dir = aim.position - bulletSpawn.position;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);

            if (rotationSpeed <= 0f) { bulletSpawn.rotation = targetRot; }
            else
            {
                float step = rotationSpeed * Time.deltaTime;
                bulletSpawn.rotation = Quaternion.RotateTowards(bulletSpawn.rotation, targetRot, step);
            }
        }

        if (_isRange == true) return;

        Vector2 enemy = gameObject.transform.position;
        Vector2 target = aim.position;
        float dist = Vector2.Distance(enemy, target);

        if (!boss)
        {
            if (dist >= distantion) { disableAttack(); }
            else { activateAttack(); }
        }
        else { activateAttack(); }
    }

    internal void activateAttack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        if (_isBlocked) return;
        if (_isBossLock) return;
        InvokeRepeating(nameof(Attack), 0f, speed);
    }

    internal void disableAttack()
    {
        if (!_isAttacking) return;
        _isAttacking = false;
        CancelInvoke(nameof(Attack));
    }

    internal void Visible_Attack() { _isBlocked = false; UnlockAttack(); }
    internal void Invisible_Attack() { CancelInvoke(nameof(Attack)); _isBlocked = true; _visionBlock = false; }

    private void UnlockAttack()
    {
        if (_visionBlock) return;
        if (_isBossLock) return;
        if (_isAttacking) { InvokeRepeating(nameof(Attack), 0f, speed); }
        _visionBlock = true;
    }

    private void Attack()
    {
        if (_isBossLock) return;
        src.PlayOneShot(sound);

        if (!_isRange)
        {
            if (isHaveCustomPunch) { CustomPunch.Invoke(); }
            else { if (meeleAnim != null) meeleAnim.Play(); }
        }
        else
        {
            if (aim != null && bulletSpawn != null)
            {
                Vector3 dir = aim.position - bulletSpawn.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
                bulletSpawn.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        }
    }
}

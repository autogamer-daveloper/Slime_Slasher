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

    [Header("Rotation")]
    [Tooltip("Градусы, чтобы подогнать ориентацию спрайта (например, если спрайт 'смотрит' вверх, поставьте 90).")]
    [SerializeField] private float rotationOffset = 0f;
    [Tooltip("Скорость плавного поворота (градусы/сек). Если 0 — поворот будет мгновенным.")]
    [SerializeField] private float rotationSpeed = 360f;

    private bool isRange = false;
    private bool isAttacking = false;

    private bool isBlocked = false;
    private bool visionBlock = false;

    private void OnEnable()
    {
        if (boss)
        {
            isBlocked = true;
            Invoke(nameof(UnlockAttacks), 7.5f);
        }
    }

    private void UnlockAttacks() { isBlocked = false; }

    internal void isRangeEnemy(bool answer)
    {
        isRange = answer;
    }

    private void Update()
    {
        if (isRange && aim != null && bulletSpawn != null)
        {
            Vector3 dir = aim.position - bulletSpawn.position;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);

            if (rotationSpeed <= 0f)
            {
                bulletSpawn.rotation = targetRot;
            }
            else
            {
                float step = rotationSpeed * Time.deltaTime;
                bulletSpawn.rotation = Quaternion.RotateTowards(bulletSpawn.rotation, targetRot, step);
            }
        }

        if (isRange == true) return;

        Vector2 enemy = gameObject.transform.position;
        Vector2 target = aim.position;
        float dist = Vector2.Distance(enemy, target);

        if (!boss)
        {
            if (dist >= distantion)
            {
                disableAttack();
            }
            else
            {
                activateAttack();
            }
        }
        else
        {
            activateAttack();
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (isRange == true) return;
    //     if (other.CompareTag("Player"))
    //     {
    //         activateAttack();
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (isRange == true) return;
    //     if (other.CompareTag("Player"))
    //     {
    //         disableAttack();
    //     }
    // }

    internal void activateAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        if (isBlocked) return;
        InvokeRepeating(nameof(Attack), 0f, speed);
    }

    internal void disableAttack()
    {
        if (!isAttacking) return;
        isAttacking = false;
        CancelInvoke(nameof(Attack));
    }

    internal void Visible_Attack() { isBlocked = false; UnlockAttack(); }
    internal void Invisible_Attack() { CancelInvoke(nameof(Attack)); isBlocked = true; visionBlock = false; }

    private void UnlockAttack()
    {
        if (visionBlock) return;
        if (isAttacking) { InvokeRepeating(nameof(Attack), 0f, speed); }
        visionBlock = true;
    }

    private void Attack()
    {
        if (!isRange)
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

using UnityEngine;

/// <summary>
/// Скрипт привешивается к объекту-триггеру (Collider2D с IsTrigger = true).
/// В поле enemyTransform указывай Transform врага, который должен двигаться.
/// Если у врага есть Rigidbody2D и ты хочешь использовать физическое перемещение, 
/// присвой его в enemyRigidbody. Иначе будет использовано прямое изменение Transform.position.
/// Добавлена логика разворачивания по X в сторону цели.
/// Теперь также корректируется масштаб hpBar, чтобы он визуально не переворачивался.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("Ray Check")]
    [SerializeField] private Transform _targetRay;

    [Header("References")]
    [Tooltip("Transform врага, который будет двигаться")]
    public Transform enemyTransform;

    [Tooltip("Опционально: Rigidbody2D врага. Если задан — движение через MovePosition (FixedUpdate).")]
    public Rigidbody2D enemyRigidbody;

    [Header("Movement")]
    [Tooltip("Скорость движения (ед./сек)")]
    public float moveSpeed = 3f;

    [Tooltip("Если true — проверяем дистанцию и останавливаемся при <= stopRange")]
    public bool isRangeEnemy = false;

    [Tooltip("Дистанция, при которой враг останавливается (если isRangeEnemy = true)")]
    public float stopRange = 5f;

    [Header("Flip (facing) settings")]
    [Tooltip("Включить автоматическое разворачивание врага в сторону цели")]
    public bool flipOnDirection = true;

    [Tooltip("Мёртвая зона по X (чтобы при почти одинаковой X не дергался масштаб)")]
    public float flipDeadzone = 0.05f;

    [Header("Attack")]
    [SerializeField] private EnemyAttack attack;

    [Header("Health Bar")]
    [SerializeField] private Transform hpBar;

    [Header("Animations")]
    [SerializeField] private bool isUsingAnims = false;
    [SerializeField] private GameObject[] animations;

    private int usingAnimation = 0;

    // публичный бул, который ты просил — можно читать/писать снаружи
    [HideInInspector]
    public bool isWalking = false;

    // приватная ссылка на Transform игрока, если он сейчас в триггере
    private Transform _playerTransform = null;

    // следующая позиция, вычисляемая для FixedUpdate или Update
    private Vector2 _nextPosition;

    // исходный масштаб врага, чтобы сохранять абсолютные значения осей
    private Vector3 _initialScale = Vector3.one;

    // --- Новое: сохраняем исходный локальный масштаб hpBar, чтобы корректно компенсировать flip ---
    private Vector3 _initialHpBarLocalScale = Vector3.one;

    private Collider2D _selfCollider;

    private bool isPlayerVisible = false;

    private void Reset()
    {
        // удобство: если скрипт повешен на того же объекта, что и Rigidbody2D врага, попробуем подставить
        if (enemyRigidbody == null && enemyTransform != null)
        {
            var rb = enemyTransform.GetComponent<Rigidbody2D>();
            if (rb != null) enemyRigidbody = rb;
        }
    }

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>();

        if (attack != null)
        {
            attack.isRangeEnemy(isRangeEnemy);
        }

        if (enemyTransform != null)
            _initialScale = enemyTransform.localScale;

        // Если hpBar задан — сохраняем его начальный локальный масштаб
        if (hpBar != null)
            _initialHpBarLocalScale = hpBar.localScale;
    }

    private void OnValidate()
    {
        // при изменениях в инспекторе - убедимся, что initialScale не нулевой
        if (enemyTransform != null && _initialScale == Vector3.zero)
            _initialScale = enemyTransform.localScale;

        // на случай правки в инспекторе — сохраним начальный масштаб hpBar, если он не нулевой
        if (hpBar != null && _initialHpBarLocalScale == Vector3.zero)
            _initialHpBarLocalScale = hpBar.localScale;
    }

    private void Update()
    {
        // если enemyTransform не задан — ничего не делаем
        if (enemyTransform == null) return;

        CheckRayToTarget();

        if (!isPlayerVisible) return;
        if (_playerTransform != null)
        {
            Vector2 enemyPos = enemyTransform.position;
            Vector2 playerPos = _playerTransform.position;
            float dist = Vector2.Distance(enemyPos, playerPos);

            // если это диапазонный враг и игрок слишком близко — стоп
            if (isRangeEnemy && dist <= stopRange)
            {
                isWalking = false;
                _nextPosition = enemyPos; // оставляем на месте
            }
            else
            {
                // движение к игроку (через Transform)
                Vector2 dir = (playerPos - enemyPos).normalized;
                _nextPosition = enemyPos + dir * moveSpeed * Time.deltaTime;

                if (enemyRigidbody == null)
                {
                    enemyTransform.position = _nextPosition;
                }
                isWalking = true;
            }

            // обновляем разворот при движении/присутствии игрока
            if (flipOnDirection)
                UpdateFlip(enemyPos, playerPos);
        }
        else
        {
            // игрок не в триггере — стоп
            isWalking = false;
        }

        if (isUsingAnims)
        {
            if (isWalking) { SetAnimation(1); }
            else { SetAnimation(0); }
        }
    }

    private void FixedUpdate()
    {
        // если задан Rigidbody2D — используй MovePosition в FixedUpdate
        if (enemyRigidbody == null) return;
        if (enemyTransform == null) return;
        if (!isPlayerVisible) return;

        if (_playerTransform != null)
        {
            Vector2 enemyPos = enemyRigidbody.position;
            Vector2 playerPos = _playerTransform.position;
            float dist = Vector2.Distance(enemyPos, playerPos);

            if (isRangeEnemy && dist <= stopRange)
            {
                isWalking = false;
                // не двигаем
            }
            else
            {
                Vector2 dir = (playerPos - enemyPos).normalized;
                Vector2 target = enemyPos + dir * moveSpeed * Time.fixedDeltaTime;
                enemyRigidbody.MovePosition(target);
                isWalking = true;
            }

            // обновляем разворот
            if (flipOnDirection)
                UpdateFlip(enemyPos, playerPos);
        }
        else
        {
            isWalking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // сохраняем ссылку на игрока
            _playerTransform = other.transform;
            if (attack != null && isRangeEnemy == true)
            {
                attack.activateAttack();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // если выходит именно тот объект, который был сохранён — убираем ссылку
            if (_playerTransform == other.transform)
            {
                _playerTransform = null;
            }
            if (attack != null && isRangeEnemy == true)
            {
                attack.disableAttack();
            }
        }
    }

    /// <summary>
    /// Обновляет localScale по X в сторону игрока.
    /// Сохраняет абсолютные значения initialScale.y и initialScale.z.
    /// Использует flipDeadzone чтобы избежать дерганий при почти равных X.
    /// Также компенсирует масштаб hpBar так, чтобы он визуально не переворачивался.
    /// </summary>
    private void UpdateFlip(Vector2 enemyPos, Vector2 playerPos)
    {
        if (enemyTransform == null) return;

        float dx = playerPos.x - enemyPos.x;

        // если в пределах мёртвой зоны — не меняем
        if (Mathf.Abs(dx) <= flipDeadzone) return;

        int desiredSign = dx < 0f ? -1 : 1;

        // текущий знак по X
        int currentSign = enemyTransform.localScale.x < 0f ? -1 : 1;

        if (currentSign != desiredSign)
        {
            Vector3 newScale = enemyTransform.localScale;
            float absInitX = Mathf.Abs(_initialScale.x);
            newScale.x = absInitX * desiredSign;
            // оставляем y и z положительными абсолютными значениями initial (чтобы не инвертировать случайно)
            newScale.y = Mathf.Abs(_initialScale.y);
            newScale.z = Mathf.Abs(_initialScale.z);
            enemyTransform.localScale = newScale;

            // --- Новое: компенсируем локальный масштаб hpBar, чтобы глобально он оставался как был ---
            if (hpBar != null)
            {
                Vector3 hpNew = _initialHpBarLocalScale;
                // умножаем на desiredSign: это компенсирует flip родителя (childLocal.x = initLocal.x * desiredSign)
                hpNew.x = _initialHpBarLocalScale.x * desiredSign;
                // сохраняем y и z из initial локального масштаба hpBar
                hpNew.y = _initialHpBarLocalScale.y;
                hpNew.z = _initialHpBarLocalScale.z;
                hpBar.localScale = hpNew;
            }
        }
    }

    // Опционально: публичный метод чтобы форсировать остановку (если захочешь)
    public void ForceStop()
    {
        _playerTransform = null;
        isWalking = false;
    }

    private void CheckRayToTarget()
    {
        if (_targetRay == null) return;

        Vector2 origin = _selfCollider != null
            ? (Vector2)_selfCollider.bounds.center
            : (Vector2)transform.position;

        Vector2 target = _targetRay.position;

        RaycastHit2D[] hits = Physics2D.LinecastAll(origin, target);

        bool blocked = false;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D col = hits[i].collider;
            if (col == null) continue;

            if (col.isTrigger) continue;

            if (col.transform == transform || col.transform.IsChildOf(enemyTransform))
                continue;

            if (col.transform == _targetRay || col.transform.IsChildOf(_targetRay))
                continue;

            blocked = true;
            break;
        }

        Debug.DrawLine(origin, target, blocked ? Color.red : Color.green);

        if (!blocked)
        {
            Visible();
        }
        else
        {
            Invisible();
        }
    }

    private void Visible()
    {
        isPlayerVisible = true;
        if (isUsingAnims)
        {
            if (isWalking) { SetAnimation(1); }
            else { SetAnimation(0); }
        }
    }

    private void Invisible()
    {
        isPlayerVisible = false;
        if (isUsingAnims) { SetAnimation(0); }
    }

    private void SetAnimation(int id)
    {
        if (id == usingAnimation) return;

        if (animations.Length != 0)
        {
            foreach (GameObject obj in animations)
            {
                obj.SetActive(false);
            }
        }

        usingAnimation = id;

        if (animations[id] != null) animations[id].SetActive(true);
    }
}

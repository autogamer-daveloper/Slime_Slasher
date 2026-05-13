using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("__ Fear __")]
    [SerializeField] private bool isFeared;

    [Header("__ Ray Check __")]
    [SerializeField] private Transform _targetRay;

    [Header("__ References __")]
    [Tooltip("Transform врага, который будет двигаться")]
    public Transform enemyTransform;

    [Tooltip("Опционально: Rigidbody2D врага. Если задан — движение через MovePosition (FixedUpdate).")]
    public Rigidbody2D enemyRigidbody;

    [Header("__ Movement __")]
    [Tooltip("Скорость движения (ед./сек)")]
    public float moveSpeed = 3f;

    [Tooltip("Если true — проверяем дистанцию и останавливаемся при <= stopRange")]
    public bool isRangeEnemy = false;

    [Tooltip("Дистанция, при которой враг останавливается (если isRangeEnemy = true)")]
    public float stopRange = 5f;

    [Header("__ Flip (facing) settings __")]
    [Tooltip("Включить автоматическое разворачивание врага в сторону цели")]
    public bool flipOnDirection = true;

    [Tooltip("Мёртвая зона по X (чтобы при почти одинаковой X не дергался масштаб)")]
    public float flipDeadzone = 0.05f;

    [Header("__ Attack __")]
    [SerializeField] private EnemyAttack attack;

    [Header("__ Health Bar __")]
    [SerializeField] private Transform hpBar;

    [Header("__ Animations __")]
    [SerializeField] private bool isUsingAnims = false;
    [SerializeField] private GameObject[] animations;

    [Header("__ Audio __")]
    [SerializeField] private AudioSource moveSrc;

    [HideInInspector]
    public bool isWalking = false;

    private int _usingAnimation = 0;
    private Transform _playerTransform = null;
    private Vector2 _nextPosition;
    private Vector3 _initialScale = Vector3.one;
    private Vector3 _initialHpBarLocalScale = Vector3.one;

    private Collider2D _selfCollider;
    private bool _isPlayerVisible = false;

    private void OnDisable() { moveSrc.mute = true; }

    private void Reset()
    {
        if (enemyRigidbody == null && enemyTransform != null)
        {
            var rb = enemyTransform.GetComponent<Rigidbody2D>();
            if (rb != null) { enemyRigidbody = rb; }
        }
    }

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>();

        if (attack != null) { attack.isRangeEnemy(isRangeEnemy); }
        if (enemyTransform != null) { _initialScale = enemyTransform.localScale; }
        if (hpBar != null) { _initialHpBarLocalScale = hpBar.localScale; }
    }

    private void OnValidate()
    {
        if (enemyTransform != null && _initialScale == Vector3.zero) { _initialScale = enemyTransform.localScale; }
        if (hpBar != null && _initialHpBarLocalScale == Vector3.zero) { _initialHpBarLocalScale = hpBar.localScale; }
    }

    private void Update()
    {
        if (enemyTransform == null) return;

        CheckRayToTarget();

        if (!_isPlayerVisible) return;

        // Past movement logic

        //if (_playerTransform != null)
        //{
        //Vector2 enemyPos = enemyTransform.position;
        //Vector2 playerPos = _playerTransform.position;
        //float dist = Vector2.Distance(enemyPos, playerPos);

        // if (isRangeEnemy && dist <= stopRange)
        // {
        //     isWalking = false;
        //     _nextPosition = enemyPos;
        // }
        // else
        // {
        //     Vector2 dir = (playerPos - enemyPos).normalized;
        //     _nextPosition = enemyPos + dir * moveSpeed * Time.deltaTime;

        //     if (enemyRigidbody == null) { enemyTransform.position = _nextPosition; }
        //     isWalking = true;
        // }

        // if (flipOnDirection) { UpdateFlip(enemyPos, playerPos); }
        //}
        //else { isWalking = false; }

        if (isWalking) { SetAnimation(1); moveSrc.mute = false; }
        else { SetAnimation(0); moveSrc.mute = true; }
    }

    private void FixedUpdate()
    {
        if (enemyRigidbody == null) return;
        if (enemyTransform == null) return;
        if (!_isPlayerVisible) return;

        if (_playerTransform != null)
        {
            Vector2 enemyPos = enemyRigidbody.position;
            Vector2 playerPos = _playerTransform.position;
            float dist = Vector2.Distance(enemyPos, playerPos);

            if (isRangeEnemy && dist <= stopRange) { isWalking = false; }
            else
            {
                Vector2 dir = (playerPos - enemyPos).normalized;
                if(isFeared) { dir = (enemyPos - playerPos).normalized; }
                Vector2 target = enemyPos + dir * moveSpeed * Time.fixedDeltaTime;
                enemyRigidbody.MovePosition(target);
                isWalking = true;
            }

            if (flipOnDirection) { UpdateFlip(enemyPos, playerPos); }
        }
        else { isWalking = false; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransform = other.transform;
            if (attack != null && isRangeEnemy == true) { attack.activateAttack(); }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_playerTransform == other.transform) { _playerTransform = null; }
            if (attack != null && isRangeEnemy == true) { attack.disableAttack(); }
        }
    }
    private void UpdateFlip(Vector2 enemyPos, Vector2 playerPos)
    {
        if (enemyTransform == null) return;
        float dx = playerPos.x - enemyPos.x;

        if (Mathf.Abs(dx) <= flipDeadzone) return;

        int desiredSign = dx < 0f ? -1 : 1;
        int currentSign = enemyTransform.localScale.x < 0f ? -1 : 1;

        if (currentSign != desiredSign)
        {
            Vector3 newScale = enemyTransform.localScale;
            float absInitX = Mathf.Abs(_initialScale.x);
            newScale.x = absInitX * desiredSign;
            newScale.y = Mathf.Abs(_initialScale.y);
            newScale.z = Mathf.Abs(_initialScale.z);
            enemyTransform.localScale = newScale;

            if (hpBar != null)
            {
                Vector3 hpNew = _initialHpBarLocalScale;
                hpNew.x = _initialHpBarLocalScale.x * desiredSign;
                hpNew.y = _initialHpBarLocalScale.y;
                hpNew.z = _initialHpBarLocalScale.z;
                hpBar.localScale = hpNew;
            }
        }
    }

    public void ForceStop()
    {
        _playerTransform = null;
        isWalking = false;
    }

    private void CheckRayToTarget()
    {
        if (isFeared) { Visible(); return; }
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

        if (!blocked) { Visible(); }
        else { Invisible(); }
    }

    private void Visible()
    {
        _isPlayerVisible = true;
        if (isWalking) { SetAnimation(1); moveSrc.mute = false; }
        else { SetAnimation(0); moveSrc.mute = true; }
        attack.Visible_Attack();
    }

    private void Invisible()
    {
        _isPlayerVisible = false;
        SetAnimation(0); moveSrc.mute = true;
        attack.Invisible_Attack();
    }

    private void SetAnimation(int id)
    {
        if (!isUsingAnims) return;
        if (id == _usingAnimation) return;

        if (animations.Length != 0) { foreach (GameObject obj in animations) { obj.SetActive(false); } }

        _usingAnimation = id;

        if (animations[id] != null) animations[id].SetActive(true);
    }
}

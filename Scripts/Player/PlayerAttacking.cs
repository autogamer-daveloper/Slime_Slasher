using UnityEngine;
using UnityEngine.UI;
using Objects.Weapons;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    [Header("__ Input Type __")]
    [SerializeField] private InputType type;

    [Header("__ Default __")]
    [SerializeField] private Button[] attackButton;
    [SerializeField] private GameObject defaultAnimation;
    [SerializeField] private Transform arrowSpawn;
    [SerializeField] private GameObject simpleAttack;
    [SerializeField] private GameObject aimJoystick;
    [SerializeField] private GameObject aim;

    [Header("__ Joystick Settings __")]
    [SerializeField] private Joystick aimStick;
    [SerializeField] private float aimMaxDistance = 2f;
    [SerializeField] private float aimDeadzone = 0.25f;
    [SerializeField] private float angleOffset = -90f;

    [Header("__ Rotation / Safety __")]
    [SerializeField] private bool lockPlayerRotation = true;
    private Quaternion _initialRotation;

    [Header("__ Player Setting __")]
    [SerializeField] private PlayerStatus player;
    [SerializeField] private CheckForEnemies enemiesAround;
    [SerializeField] private Button[] selectWeaponById;

    [Header("__ Meele Effect Aim __")]
    [SerializeField] private Transform aimVector;

    [Header("__ Weapons __")]
    [SerializeField] private Weapons[] weapon;

    [Header("__ Weapon Pictures __")]
    [SerializeField] private GameObject[] weaponPictures;

    [Header("__ Audio __")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectItem;

    private Animation defaultMeeleAnimation;

    private bool _isMutedFirst = true;
    private int _selectedWeapon = 0;
    private Objects.Weapons.Weapons.WeaponType _type;

    [HideInInspector] public int damage = 1;

    private float _cooldown = 0.5f;
    private int _manaCost = 1;
    private int _lifeCost = 0;
    private bool _isAttackBlocked = false;
    private AudioClip _sfx;

    // meele params
    private bool _customAnimationMeele = false;
    private string _meeleAnimationName = "Default";

    private bool _isHaveEffect = false;
    private bool _isRotatable = false;
    private GameObject _meeleEffect;

    private int _meele_extra_damage = 0;

    // range params
    private GameObject _arrow;
    private float _speed = 10f;
    private float _lifeTime = 5f;

    private bool _isUsingRange = false;

    private int _range_extra_damage = 0;

    // magic params
    private bool _isDamageWeapon = true;
    private GameObject _effect;
    [HideInInspector] public int _manaHealing = 0;
    [HideInInspector] public int _lifeHealing = 0;

    private int _mage_extra_damage = 0;

    private Vector2 _lastAimDir = Vector2.right;

    private bool isMobileInput = false;
    private bool dialogueBlock = false;

    private void Awake()
    {
        if (type != null) { isMobileInput = type.IsMobileInput(); }
        else { isMobileInput = true; }

        if (!isMobileInput)
        {
            simpleAttack.SetActive(false);
            aimJoystick.SetActive(false);
        }
    }

    private void Start()
    {
        _initialRotation = transform.rotation;

        for (int i = 0; i < selectWeaponById.Length; i++)
        {
            int index = i;
            selectWeaponById[i].onClick.AddListener(() =>
            {
                Debug.Log("AddListener id:" + index);
                SelectThisWeapon(index);
            });
        }

        if (enemiesAround == null) { enemiesAround = gameObject.AddComponent<CheckForEnemies>(); }

        if (weapon == null || weapon.Length == 0)
        {
            Debug.LogError("PlayerAttacking: weapon array is empty! Fill it in inspector.");
            return;
        }

        int savedId = KeyManager.GetInt_WeaponID();
        if (savedId < 0 || savedId >= weapon.Length)
        {
            Debug.LogWarning($"Saved weapon id {savedId} is out of range (0..{weapon.Length - 1}). Clamping to valid range.");
        }

        _selectedWeapon = Mathf.Clamp(savedId, 0, weapon.Length - 1);

        if (defaultAnimation != null) { defaultMeeleAnimation = defaultAnimation.GetComponent<Animation>(); }
        else { Debug.LogWarning("defaultAnimation GameObject is not assigned in inspector."); }

        UpdateSkills();
        SelectThisWeapon(_selectedWeapon);
    }

    private void OnDestroy() { foreach (Button btn in selectWeaponById) { if (btn != null) { btn.onClick.RemoveAllListeners(); } } }

    public void BoughtWeapon(int id) { SelectThisWeapon(id); }

    private void SelectThisWeapon(int id)
    {
        Debug.Log("Selected Weapon:" + id);

        if (weapon == null || weapon.Length == 0)
        {
            Debug.LogError("SelectThisWeapon: weapon array is empty or null.");
            return;
        }
        if (id < 0 || id >= weapon.Length)
        {
            Debug.LogError($"SelectThisWeapon: id {id} out of range (weapon.Length = {weapon.Length}).");
            return;
        }

        KeyManager.SetInt_WeaponID(id);

        var w = weapon[id];
        if (w == null) { Debug.LogError($"weapon[{id}] is null!"); return; }

        _selectedWeapon = w.WeaponId;
        _manaCost = w.ManaCost;
        _lifeCost = w.LifeCost;
        _cooldown = w.Cooldown;
        _type = w.Type;
        _sfx = w.Sfx;
        SelectWeaponPicture();

        if (w.Type == Objects.Weapons.Weapons.WeaponType.Meele)
        {
            damage = w.Damage + _meele_extra_damage;
            _customAnimationMeele = w.CustomMeeleAnimation;
            _meeleAnimationName = w.MeeleAnimationName;
            _manaHealing = 0;
            _lifeHealing = 0;
            _isHaveEffect = w.isHaveEffect;
            _isRotatable = w.isRotatable;
            _meeleEffect = w.meeleEffect;
            isWeaponRange(false);
        }
        else if (w.Type == Objects.Weapons.Weapons.WeaponType.Range)
        {
            damage = w.Damage + _range_extra_damage;
            _arrow = w.Arrow;
            _speed = w.Speed;
            _lifeTime = w.LifeTime;
            _manaHealing = 0;
            _lifeHealing = 0;
            _isHaveEffect = false;
            _isRotatable = false;
            _meeleEffect = null;
            isWeaponRange(true);
        }
        else if (w.Type == Objects.Weapons.Weapons.WeaponType.Magic)
        {
            damage = w.Damage + _mage_extra_damage;
            _customAnimationMeele = w.CustomMeeleAnimation;
            _meeleAnimationName = w.MeeleAnimationName;
            _isDamageWeapon = w.IsDamageWeapon;
            _effect = w.Effect;
            _manaHealing = w.ManaHealing;
            _lifeHealing = w.LifeHealing;
            _isHaveEffect = false;
            _isRotatable = false;
            _meeleEffect = null;
            isWeaponRange(false);
        }

        if (player != null) { player.SetBonus(_lifeHealing, _manaHealing); }
        else { Debug.LogWarning("Player reference is null in PlayerAttacking (set it in inspector)."); }

        if (_isMutedFirst) { _isMutedFirst = false; return; }
        audioSource.PlayOneShot(selectItem);
    }

    private void isWeaponRange(bool answer)
    {
        _isUsingRange = answer;
        if (answer)
        {
            if (isMobileInput)
            {
                simpleAttack.SetActive(false);
                aimJoystick.SetActive(true);
            }
            aim.SetActive(true);

            if (aim != null) { aim.transform.position = transform.position + Vector3.up * aimMaxDistance; }
        }
        else
        {
            if (isMobileInput)
            {
                simpleAttack.SetActive(true);
                aimJoystick.SetActive(false);
            }
            aim.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (attackButton.Length != 0) { foreach (Button btn in attackButton) { btn.onClick.AddListener(Attack); } }
        else { Debug.LogError("attackButton not assigned in inspector."); }
    }

    private void OnDisable() { if (attackButton.Length != 0) { foreach (Button btn in attackButton) { btn.onClick.RemoveListener(Attack); } } }

    private void Update()
    {
        if (!isMobileInput && Mouse.current.rightButton.wasPressedThisFrame) { Attack(); }
        if (!_isUsingRange) { return; }

        if (isMobileInput) { HandleAimWithJoystick(); }
        else { HandleAimWithMouse(); }
    }

    private void LateUpdate() { if (lockPlayerRotation) { transform.rotation = _initialRotation; } }

    private void HandleAimWithJoystick()
    {
        if (aimStick != null)
        {
            Vector2 input = new Vector2(aimStick.Horizontal, aimStick.Vertical);

            if (input.sqrMagnitude > aimDeadzone * aimDeadzone)
            {
                Vector2 norm = input.normalized;
                _lastAimDir = norm;

                if (aim != null)
                {
                    Vector3 worldPos = transform.position + new Vector3(norm.x, norm.y, 0f) * aimMaxDistance;
                    aim.transform.position = worldPos;
                }

                if (arrowSpawn != null)
                {
                    float angle = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;
                    arrowSpawn.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                }
            }
            else
            {
                if (aim != null)
                {
                    Vector3 worldPos = transform.position + new Vector3(_lastAimDir.x, _lastAimDir.y, 0f) * aimMaxDistance;
                    aim.transform.position = worldPos;
                }
                if (arrowSpawn != null)
                {
                    float angle = Mathf.Atan2(_lastAimDir.y, _lastAimDir.x) * Mathf.Rad2Deg;
                    arrowSpawn.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                }
            }
        }
        else
        {
            if (Camera.main == null) return;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Vector3 dir = mouseWorld - transform.position;
            if (dir.sqrMagnitude > 0.001f)
            {
                Vector2 norm = dir.normalized;
                _lastAimDir = norm;
                if (aim != null) { aim.transform.position = transform.position + (Vector3)(norm * aimMaxDistance); }

                if (arrowSpawn != null)
                {
                    float angle = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;
                    arrowSpawn.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                }
            }
        }
    }

    private void HandleAimWithMouse()
    {
        if (Camera.main == null || Mouse.current == null) { return; }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorld.z = 0f;

        Vector2 direction = mouseWorld - transform.position;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Vector2 normalizedDirection = direction.normalized;

        _lastAimDir = normalizedDirection;

        if (aim != null)
        {
            aim.transform.position =
                transform.position +
                (Vector3)(normalizedDirection * aimMaxDistance);
        }

        if (arrowSpawn != null)
        {
            float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;

            arrowSpawn.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
        }
    }

    private void Attack()
    {
        if (dialogueBlock) { return; }
        if (_isAttackBlocked) return;
        if (player == null) { Debug.LogError("Player not assigned!"); return; }

        int _mana = player.mana;
        if (_mana < _manaCost) { return; }

        if (_type == Objects.Weapons.Weapons.WeaponType.Meele)
        {
            if (defaultMeeleAnimation != null && !defaultMeeleAnimation.IsPlaying(_meeleAnimationName))
            {
                defaultMeeleAnimation.Play(_meeleAnimationName);
            }

            if (_isHaveEffect == false) { return; }

            if (_isRotatable) { Instantiate(_meeleEffect, aimVector.position, aimVector.rotation); }
            else { Instantiate(_meeleEffect, aimVector.position, this.gameObject.transform.rotation); }
        }

        if (_type == Objects.Weapons.Weapons.WeaponType.Range)
        {
            if (_arrow != null && arrowSpawn != null)
            {
                GameObject inst = Instantiate(_arrow, arrowSpawn.position, arrowSpawn.rotation);
                Rigidbody2D rb2d = inst.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    Vector2 forward = arrowSpawn.up;
                    rb2d.linearVelocity = forward * _speed;
                }
                Destroy(inst, _lifeTime);
            }
            else { Debug.LogWarning("Arrow or arrowSpawn not set for Range weapon."); }
        }

        if (_type == Objects.Weapons.Weapons.WeaponType.Magic)
        {
            if (_isDamageWeapon)
            {
                bool haveEnemiesAround = enemiesAround.isHaveEnemiesAround();

                if(!haveEnemiesAround) { return; }

                Transform[] enemiesTransform = enemiesAround.GetAllEnemiesTransform();
                foreach (Transform enemyTransform in enemiesTransform)
                {
                    if (_effect != null) { Instantiate(_effect, enemyTransform.position, _effect.transform.rotation); }
                }
            }
            else
            {
                if (defaultMeeleAnimation != null && !defaultMeeleAnimation.IsPlaying(_meeleAnimationName))
                {
                    defaultMeeleAnimation.Play(_meeleAnimationName);
                }
            }
        }

        if (_sfx != null && audioSource != null) { audioSource.PlayOneShot(_sfx); }
        player.ManaLose(_manaCost, _lifeCost);
        _isAttackBlocked = true;
        Invoke("UnlockAttack", _cooldown);
    }

    private void UnlockAttack() => _isAttackBlocked = false;

    internal void UpdateSkills()
    {
        _meele_extra_damage = KeyManager.Get_Bool_Key("skill_MEELE");
        _range_extra_damage = KeyManager.Get_Bool_Key("skill_RANGE");
        _mage_extra_damage = KeyManager.Get_Bool_Key("skill_MAGE");

        int id = KeyManager.GetInt_WeaponID();
        var w = weapon[id];
        if (w == null) { Debug.LogError($"weapon[{id}] is null!"); return; }

        if (w.Type == Objects.Weapons.Weapons.WeaponType.Meele) { damage = w.Damage + _meele_extra_damage; }
        else if (w.Type == Objects.Weapons.Weapons.WeaponType.Range) { damage = w.Damage + _range_extra_damage; }
        else if (w.Type == Objects.Weapons.Weapons.WeaponType.Magic) { damage = w.Damage + _mage_extra_damage; }
    }

    private void SelectWeaponPicture() { foreach (GameObject obj in weaponPictures) { obj.SetActive(false); } weaponPictures[_selectedWeapon].SetActive(true); }

    internal void DialogueBlock(bool status) { dialogueBlock = status; }
}

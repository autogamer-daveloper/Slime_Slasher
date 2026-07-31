using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Objects.Accessories;
using TMPro;

[System.Serializable]
public class Accessories
{
    public Accessory accessory;
    public Button selectButton;
}

public class PlayerStatus : MonoBehaviour
{
    [Header("__ Scene ID __")]
    [SerializeField] private int sceneID = 0;
    [SerializeField] private bool isZero = false;
    [Header("__ Special __")]
    [SerializeField] private bool isFirstNightmare = false;
    [SerializeField] private ActivateTrapsNightmare nightmare;
    [Header("__ UI __")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [Header("__ Accessories __")]
    public Accessories[] accessories;
    [Header("__ Accessories Picture __")]
    [SerializeField] private GameObject[] accessoriesPictures;
    [Header("__ Dead __")]
    [SerializeField] private GameObject[] deadPanels;
    [SerializeField] private Animation[] deadAnims;
    private int _deathCutsceneIndex = 13;
    [SerializeField] private UnityEvent clearEnemies;
    [SerializeField] private Transform spawnPoint;
    [Header("__ Textures __")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private GameObject alivePlayer;
    [SerializeField] private GameObject deadPlayer;
    [Header("__ Audio __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip selectItem;
    [SerializeField] private AudioClip mage;

    private int health = 100;
    [HideInInspector] public int mana = 100;

    private int maxHealth = 100;
    private int maxMana = 100;

    private int healthRegen = 5;
    private int manaRegen = 4;

    private int boostHealthRegen = 0;
    private int boostManaRegen = 0;

    private bool isBlocked = false;
    private bool isDead = false;

    //accessories
    private int _extraLife = 0;
    private int _extraRegen = 0;

    private int _extraMana = 0;
    private int _extraManaRegen = 0;

    //lifes
    private int _isEasyMode = 0;
    private int _deathCount = 0;

    //skill tree
    private int _st_health = 0;
    private int _st_mana = 0;

    private void Start()
    {
        SetAccessoriesButtons();
        CheckAccessories();
        UpdateSkills();

        healthSlider.maxValue = maxHealth + _extraLife + _st_health;
        manaSlider.maxValue = maxMana + _extraMana + _st_mana;

        healthSlider.value = health + _extraLife + _st_health;
        manaSlider.value = mana + _extraMana + _st_mana;

        if (isZero == true)
        {
            maxHealth = 1;
            maxMana = 1;

            health = 1;
            mana = 1;

            healthSlider.maxValue = 1;
            manaSlider.maxValue = 1;

            healthSlider.value = 1;
            manaSlider.value = 1;

            //healthText.text = health.ToString() + "/" + maxHealth.ToString();
            //manaText.text = mana.ToString() + "/" + maxMana.ToString();
            
            healthText.text = "Inf";
            manaText.text = "Inf";
        }

        Regeneration();
        InvokeRepeating("Regeneration", 1, 1);

        _isEasyMode = KeyManager.Get_Bool_Key("easyMode");
        if (isZero != true)
        {
            _deathCount = KeyManager.Get_Bool_Key("deathCount");
            if (_deathCount >= 3)
            {
                deadPanels[2].SetActive(true);
                deadAnims[2].Play();
                Invoke(nameof(HideDeadPanels), 4.5f);
                Invoke(nameof(DeadCutscene), 4.5f);
            }
        }
    }

    private void OnDestroy()
    {
        if (accessories == null) return;
        for (int i = 0; i < accessories.Length; i++)
        {
            if (accessories[i] != null && accessories[i].selectButton != null) { accessories[i].selectButton.onClick.RemoveAllListeners(); }
        }
    }

    private void Regeneration()
    {
        if(isZero == true) { return; }
        health += healthRegen + boostHealthRegen + _extraRegen;
        mana += manaRegen + boostManaRegen + _extraManaRegen;

        if (health >= maxHealth + _extraLife + _st_health) { health = maxHealth + _extraLife + _st_health; }
        if (mana >= maxMana + _extraMana + _st_mana) { mana = maxMana + _extraMana + _st_mana; }

        healthText.text = health.ToString() + "/" + (maxHealth + _extraLife + _st_health).ToString();
        manaText.text = mana.ToString() + "/" + (maxMana + _extraMana + _st_mana).ToString();

        healthSlider.value = health;
        manaSlider.value = mana;
        DeathCheck(true);
    }

    private void GetExtraMana(int extraMana)
    {
        if(isZero == true) { return; }
        src.PlayOneShot(mage);
        mana += extraMana;
        if (mana >= maxMana + _extraMana + _st_mana) { mana = maxMana + _extraMana + _st_mana; }
        manaText.text = mana.ToString() + "/" + (maxMana + _extraMana + _st_mana).ToString();
        manaSlider.value = mana;
    }

    private void GetExtraLife(int extraLife)
    {
        if(isZero == true) { return; }
        health += extraLife;
        if (health >= maxHealth + _extraLife + _st_health) { health = maxHealth + _extraLife + _st_health; }
        healthText.text = health.ToString() + "/" + (maxHealth + _extraLife + _st_health).ToString();
        healthSlider.value = health;
    }

    internal void SetBonus(int lifeBonus, int manaBonus)
    {
        if(isZero == true) { return; }
        boostHealthRegen = lifeBonus;
        boostManaRegen = manaBonus;
    }

    internal void ManaLose(int count, int lifeCount)
    {
        if(isZero == true) { return; }
        mana -= count;
        health -= lifeCount;
        healthText.text = health.ToString() + "/" + (maxHealth + _extraLife + _st_health).ToString();
        manaText.text = mana.ToString() + "/" + (maxMana + _extraMana + _st_mana).ToString();

        healthSlider.value = health;
        manaSlider.value = mana;
        DeathCheck(true);
    }

    internal void GetDamage(int dmg)
    {
        if(isZero == true) { return; }
        health -= dmg;
        healthText.text = health.ToString() + "/" + (maxHealth + _extraLife + _st_health).ToString();

        healthSlider.value = health;
        DeathCheck(false);
    }

    private void DeathCheck(bool selfHarm)
    {
        if(isZero == true) { return; }
        if (health <= 0)
        {
            src.PlayOneShot(death);
            Debug.Log("You died!");
            alivePlayer.SetActive(false);
            deadPlayer.SetActive(true);
            if (isFirstNightmare == true)
            {
                if (nightmare != null) { nightmare.DeadSlime(); }
                controller.LockInput();
                clearEnemies.Invoke();
            }
            else
            {
                isDead = true;
                controller.LockInput();
                if (_isEasyMode == 0)
                {
                    if (_deathCount < 0)
                    {
                        deadPanels[_deathCount].SetActive(true);
                        deadAnims[_deathCount].Play();
                        Invoke(nameof(HideDeadPanels), 6f);
                        _deathCount = 0;
                    }
                    if (_deathCount < 2)
                    {
                        deadPanels[_deathCount].SetActive(true);
                        deadAnims[_deathCount].Play();
                        Invoke(nameof(HideDeadPanels), 6f);
                    }
                    else
                    {
                        deadPanels[2].SetActive(true);
                        deadAnims[2].Play();
                        KeyManager.Delete_All();
                        Invoke(nameof(DeadCutscene), 6f);
                    }

                    Invoke(nameof(Clear), 1f);
                    Invoke(nameof(Teleport), 1f);
                    _deathCount++;
                    KeyManager.Set_Bool_Key("deathCount", _deathCount);
                }
                else
                {
                    deadPanels[0].SetActive(true);
                    deadAnims[0].Play();
                    Invoke(nameof(HideDeadPanels), 6f);
                    Invoke(nameof(Clear), 1f);
                    Invoke(nameof(Teleport), 1f);
                }
            }
        }
        else { if (!selfHarm) { src.PlayOneShot(damage); } }
    }

    private void Clear()
    {
        if(isZero == true) { return; }
        clearEnemies.Invoke();

        health = 100;
        mana = 100;

        healthSlider.maxValue = maxHealth + _extraLife + _st_health;
        manaSlider.maxValue = maxMana + _extraMana + _st_mana;

        healthSlider.value = health + _extraLife + _st_health;
        manaSlider.value = mana + _extraMana + _st_mana;
    }

    private void DeadCutscene() { if(isZero) { return; } LoadLevel.LoadLevelById(_deathCutsceneIndex); }

    private void Teleport()
    {
        if(isZero == true) { return; }
        gameObject.transform.position = spawnPoint.position;
        alivePlayer.SetActive(true);
        deadPlayer.SetActive(false);
    }

    //private void HideDeadPanels() { foreach (GameObject obj in deadPanels) { obj.SetActive(false); }} OLD
    private void HideDeadPanels() { LoadLevel.LoadLevelById(sceneID); } //NEW

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isZero == true) { return; }
        if (isDead == true) return;
        if (isBlocked == true) return;
        if (other.CompareTag("EnemyAttack"))
        {
            EnemyBullet attack = other.GetComponent<EnemyBullet>();
            if (attack != null)
            {
                GetDamage(attack.Damage);
                isBlocked = true;
                Invoke(nameof(UnlockDamage), 0.1f);
            }
        }
        if (other.CompareTag("ExtraMana"))
        {
            ExtraManaPoint point = other.GetComponent<ExtraManaPoint>();
            if (point != null)
            {
                int tempMana = point.CountOfExtraMana();
                GetExtraMana(tempMana);
                int tempLife = point.CountOfExtraLife();
                GetExtraLife(tempLife);
                point.DestroyThisPoint();
            }
        }
    }

    private void UnlockDamage() { isBlocked = false; }

    private void CheckAccessories()
    {
        if(isZero == true) { return; }
        if (accessories == null || accessories.Length == 0)
        {
            Debug.LogWarning("[PlayerStatus] accessories array is null or empty. No accessory bonuses applied.");
            _extraLife = _extraRegen = _extraMana = _extraManaRegen = 0;
            return;
        }

        int i = KeyManager.GetInt_AccessoryID();

        if (i < 0 || i >= accessories.Length)
        {
            Debug.LogWarning($"[PlayerStatus] AccessoryID {i} out of range (0..{accessories.Length - 1}). Clamping to 0.");
            i = Mathf.Clamp(i, 0, accessories.Length - 1);
            try { KeyManager.SetInt_AccessoryID(i); }
            catch { }
        }

        if (accessories[i] == null || accessories[i].accessory == null)
        {
            Debug.LogWarning($"[PlayerStatus] accessories[{i}] or its accessory is null. No accessory bonuses applied.");
            _extraLife = _extraRegen = _extraMana = _extraManaRegen = 0;
            return;
        }

        SelectAccessoriesPicture(i);

        _extraLife = accessories[i].accessory.extraLife;
        _extraRegen = accessories[i].accessory.extraRegen;
        _extraMana = accessories[i].accessory.extraMana;
        _extraManaRegen = accessories[i].accessory.extraManaRegen;

        health = maxHealth + _extraLife + _st_health; //New mechanic
        mana = maxMana + _extraMana + _st_mana;

        healthSlider.maxValue = maxHealth + _extraLife + _st_health;
        manaSlider.maxValue = maxMana + _extraMana + _st_mana;
    }

    private void SetAccessoriesButtons()
    {
        if (accessories == null) return;

        for (int i = 0; i < accessories.Length; i++)
        {
            int id = i;
            if (accessories[id] != null && accessories[id].selectButton != null)
            {
                accessories[id].selectButton.onClick.RemoveAllListeners();

                accessories[id].selectButton.onClick.AddListener(() =>
                {
                    KeyManager.SetInt_AccessoryID(id);
                    CheckAccessories();
                    src.PlayOneShot(selectItem);
                });
            }
        }
    }

    internal void UpdateSkills()
    {
        if(isZero == true) { return; }
        _st_health = KeyManager.Get_Bool_Key("skill_HEALTH");
        _st_mana = KeyManager.Get_Bool_Key("skill_MANA");
    }

    private void SelectAccessoriesPicture(int id) { foreach(GameObject obj in accessoriesPictures) { obj.SetActive(false); } accessoriesPictures[id].SetActive(true); }
}
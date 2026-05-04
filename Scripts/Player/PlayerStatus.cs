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
    [Header("__ Special __")]
    [SerializeField] private bool isFirstNightmare = false;
    [SerializeField] private ActivateTrapsNightmare nightmare;
    [Header("__ UI __")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [Header("__ Accesories __")]
    public Accessories[] accessories;
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
    private int manaRegen = 1;

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
    private int isEasyMode = 0;
    private int deathCount = 0;

    private void Start()
    {
        SetAccessoriesButtons();
        CheckAccessories();

        healthSlider.maxValue = maxHealth + _extraLife;
        manaSlider.maxValue = maxMana + _extraMana;

        healthSlider.value = health + _extraLife;
        manaSlider.value = mana + _extraMana;

        Regeneration();
        InvokeRepeating("Regeneration", 1, 1);

        isEasyMode = KeyManager.Get_Bool_Key("easyMode");
        deathCount = KeyManager.Get_Bool_Key("deathCount");
        if (deathCount >= 3)
        {
            deadPanels[2].SetActive(true);
            deadAnims[2].Play();
            Invoke(nameof(HideDeadPanels), 4.5f);
            Invoke(nameof(DeadCutscene), 4.5f);
        }
    }

    private void OnDestroy()
    {
        if (accessories == null) return;

        for (int i = 0; i < accessories.Length; i++)
        {
            if (accessories[i] != null && accessories[i].selectButton != null)
            {
                accessories[i].selectButton.onClick.RemoveAllListeners();
            }
        }
    }

    private void Regeneration()
    {
        health += healthRegen + boostHealthRegen + _extraRegen;
        mana += manaRegen + boostManaRegen + _extraManaRegen;

        if (health >= maxHealth + _extraLife)
        {
            health = maxHealth + _extraLife;
        }

        if (mana >= maxMana + _extraMana)
        {
            mana = maxMana + _extraMana;
        }

        healthText.text = (maxHealth + _extraLife).ToString() + "/" + health.ToString();
        manaText.text = (maxMana + _extraMana).ToString() + "/" + mana.ToString();

        healthSlider.value = health;
        manaSlider.value = mana;
    }

    private void GetExtraMana(int extraMana)
    {
        src.PlayOneShot(mage);
        mana += extraMana;
        if (mana >= maxMana + _extraMana) { mana = maxMana + _extraMana; }
        manaText.text = (maxMana + _extraMana).ToString() + "/" + mana.ToString();
    }

    private void GetExtraLife(int extraLife)
    {
        health += extraLife;
        if (health >= maxHealth + _extraLife) { health = maxHealth + _extraLife; }
        healthText.text = (maxHealth + _extraLife).ToString() + "/" + health.ToString();
    }

    internal void SetBonus(int lifeBonus, int manaBonus)
    {
        boostHealthRegen = lifeBonus;
        boostManaRegen = manaBonus;
    }

    internal void ManaLose(int count, int lifeCount)
    {
        mana -= count;
        health -= lifeCount;
        healthText.text = (maxHealth + _extraLife).ToString() + "/" + health.ToString();
        manaText.text = (maxMana + _extraMana).ToString() + "/" + mana.ToString();

        healthSlider.value = health;
        manaSlider.value = mana;
        DeathCheck(true);
    }

    internal void GetDamage(int dmg)
    {
        health -= dmg;
        healthText.text = (maxHealth + _extraLife).ToString() + "/" + health.ToString();

        healthSlider.value = health;

        DeathCheck(false);
    }

    private void DeathCheck(bool selfHarm)
    {
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
                if (isEasyMode == 0)
                {
                    if (deathCount < 0)
                    {
                        deadPanels[deathCount].SetActive(true);
                        deadAnims[deathCount].Play();
                        Invoke(nameof(HideDeadPanels), 5.5f);

                        deathCount = 0;
                    }
                    if (deathCount < 2)
                    {
                        deadPanels[deathCount].SetActive(true);
                        deadAnims[deathCount].Play();
                        Invoke(nameof(HideDeadPanels), 5.5f);
                    }
                    else
                    {
                        deadPanels[2].SetActive(true);
                        deadAnims[2].Play();
                        KeyManager.Delete_All();
                        Invoke(nameof(HideDeadPanels), 4f);
                        Invoke(nameof(DeadCutscene), 4f);
                    }

                    Invoke(nameof(Clear), 1f);
                    Invoke(nameof(Teleport), 1f);
                    Invoke(nameof(UnlockDamageByDead), 5f);
                    deathCount++;
                    KeyManager.Set_Bool_Key("deathCount", deathCount);
                }
                else
                {
                    deadPanels[0].SetActive(true);
                    deadAnims[0].Play();
                    Invoke(nameof(HideDeadPanels), 5.5f);

                    Invoke(nameof(Clear), 1f);
                    Invoke(nameof(UnlockDamageByDead), 5f);
                    Invoke(nameof(Teleport), 1f);
                }
            }
        }
        else
        {
            if (!selfHarm) { src.PlayOneShot(damage); }
        }
    }

    private void Clear()
    {
        clearEnemies.Invoke();

        health = 100;
        mana = 100;

        healthSlider.maxValue = maxHealth + _extraLife;
        manaSlider.maxValue = maxMana + _extraMana;

        healthSlider.value = health + _extraLife;
        manaSlider.value = mana + _extraMana;
    }

    private void DeadCutscene() { LoadLevel.LoadLevelById(_deathCutsceneIndex); }

    private void Teleport()
    {
        gameObject.transform.position = spawnPoint.position;
        alivePlayer.SetActive(true);
        deadPlayer.SetActive(false);
    }

    private void HideDeadPanels()
    {
        foreach (GameObject obj in deadPanels)
        {
            obj.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

    private void UnlockDamage()
    {
        isBlocked = false;
    }

    private void UnlockDamageByDead()
    {
        isDead = false;
        controller.UnlockInput();
    }

    private void CheckAccessories()
    {
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
            try
            {
                KeyManager.SetInt_AccessoryID(i);
            }
            catch
            {
            }
        }

        if (accessories[i] == null || accessories[i].accessory == null)
        {
            Debug.LogWarning($"[PlayerStatus] accessories[{i}] or its accessory is null. No accessory bonuses applied.");
            _extraLife = _extraRegen = _extraMana = _extraManaRegen = 0;
            return;
        }

        _extraLife = accessories[i].accessory.extraLife;
        _extraRegen = accessories[i].accessory.extraRegen;
        _extraMana = accessories[i].accessory.extraMana;
        _extraManaRegen = accessories[i].accessory.extraManaRegen;

        healthSlider.maxValue = maxHealth + _extraLife;
        manaSlider.maxValue = maxMana + _extraMana;
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
}
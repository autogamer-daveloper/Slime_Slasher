using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class HealthEvents
{
    public int health = 350;
    public UnityEvent action;
    [HideInInspector] public bool isActed = false;
}

public class EnemyStats : MonoBehaviour
{
    [Header("__ Player __")]
    [SerializeField] private PlayerAttacking player;
    [SerializeField] private GameObject toDestroy;
    [SerializeField] private GameObject deadBody;
    [SerializeField] private UnityEvent action;
    [SerializeField] private UnityEvent damaged;
    [Header("__ Enemy Settings __")]
    [SerializeField] private int maxHealth = 15;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private bool autoUnlock = true;
    [Header("__ Boss fight __")]
    [SerializeField] private bool needSave = false;
    [SerializeField] private HealthEvents[] healthEvents;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip dmg;

    private int _health = 15;
    private bool _blockedDamage = false;

    private void OnEnable()
    {
        healthBar.maxValue = maxHealth;
        _health = maxHealth;
        healthBar.value = _health;
        healthText.text = maxHealth.ToString() + "/" + _health.ToString();

        _blockedDamage = true;
        if (autoUnlock) Invoke("UnlockDamage", 0.2f);
    }

    private void GetDamage(int damage)
    {
        if (_blockedDamage) return;
        Debug.Log("Damaged");
        _health -= damage;
        if (healthEvents != null)
        {
            foreach (HealthEvents events in healthEvents)
            {
                if (_health <= events.health)
                {
                    if (events.isActed == false)
                    {
                        events.isActed = true;
                        events.action.Invoke();
                    }
                }
            }
        }

        if (_health > 0)
        {
            healthBar.value = _health;
            healthText.text = maxHealth.ToString() + "/" + _health.ToString();
            src.PlayOneShot(dmg);
        }
        else
        {
            healthBar.value = 0;
            healthText.text = maxHealth.ToString() + "/" + "0";
            action.Invoke();

            if (needSave) return;

            GameObject temp;
            if (deadBody != null)
            {
                temp = Instantiate(deadBody, toDestroy.transform.position, toDestroy.transform.rotation);
                Vector3 deadScale = new Vector3(toDestroy.transform.localScale.x, toDestroy.transform.localScale.y, 1);
                temp.transform.localScale = deadScale;
            }
            Destroy(toDestroy);

            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_blockedDamage) return;
        Debug.Log("Triggered");
        if (other.tag == "PlayerAttack")
        {
            GetDamage(player.damage);
            _blockedDamage = true;
            Invoke("UnlockDamage", 0.2f);
        }
    }

    internal void UnlockDamage()
    {
        _blockedDamage = false;
        damaged.Invoke();
    }

    internal void BlockDamage() { _blockedDamage = true; }

    internal void ResetHealth() { maxHealth = _health; healthText.text = maxHealth.ToString() + "/" + _health.ToString(); healthBar.value = _health; }
}

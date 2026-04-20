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

    private int health = 15;
    private bool blockedDamage = false;

    private void OnEnable()
    {
        healthBar.maxValue = maxHealth;
        health = maxHealth;
        healthBar.value = health;
        healthText.text = maxHealth.ToString() + "/" + health.ToString();

        blockedDamage = true;
        if (autoUnlock) Invoke("UnlockDamage", 0.2f);
    }

    private void GetDamage(int damage)
    {
        if (blockedDamage) return;
        Debug.Log("Damaged");
        health -= damage;
        if (healthEvents != null)
        {
            foreach (HealthEvents events in healthEvents)
            {
                if (health <= events.health)
                {
                    if (events.isActed == false)
                    {
                        events.isActed = true;
                        events.action.Invoke();
                    }
                }
            }
        }

        if (health > 0)
        {
            healthBar.value = health;
            healthText.text = maxHealth.ToString() + "/" + health.ToString();
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
        if (blockedDamage) return;
        Debug.Log("Triggered");
        if (other.tag == "PlayerAttack")
        {
            GetDamage(player.damage);
            blockedDamage = true;
            Invoke("UnlockDamage", 0.2f);
        }
    }

    internal void UnlockDamage()
    {
        blockedDamage = false;
        damaged.Invoke();
    }

    internal void BlockDamage()
    {
        blockedDamage = true;
    }
}

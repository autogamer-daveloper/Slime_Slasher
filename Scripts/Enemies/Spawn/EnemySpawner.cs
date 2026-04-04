using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private GameObject prototype;
    [SerializeField] private float cooldown = 30f;

    private bool isBlocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isBlocked) return;

            isBlocked = true;
            Instantiate(prototype, gameObject.transform.position, gameObject.transform.rotation);
            Invoke(nameof(Unlock), cooldown);
        }
    }

    private void Unlock()
    {
        isBlocked = false;
    }
}

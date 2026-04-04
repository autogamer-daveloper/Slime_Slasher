using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class CheckForEnemies : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private bool debugLog = false;

    private readonly List<Transform> enemies = new List<Transform>();

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) { col.isTrigger = true; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(enemyTag)) return;

        Transform t = other.transform;

        if (!enemies.Contains(t))
        {
            enemies.Add(t);
            if (debugLog) { Debug.Log($"Enemy entered: {t.name}", t); }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(enemyTag)) return;

        Transform t = other.transform;

        if (enemies.Remove(t))
        {
            if (debugLog) { Debug.Log($"Enemy exited: {t.name}", t); }
        }
    }

    public Transform[] GetAllEnemiesTransform()
    {
        // Чистим null, если враг был уничтожен
        enemies.RemoveAll(t => t == null);
        return enemies.ToArray();
    }

    public bool IsEnemyInside(Transform enemyTransform)
    {
        return enemies.Contains(enemyTransform);
    }
}

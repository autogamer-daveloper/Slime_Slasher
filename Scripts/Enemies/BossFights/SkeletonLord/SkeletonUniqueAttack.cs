using UnityEngine;
using System.Collections;

[System.Serializable]
public class CoilSpawnPoints
{
    public Transform[] point;
}

public class SkeletonUniqueAttack : MonoBehaviour
{
    [Header("__ Spawn points __")]
    [SerializeField] private CoilSpawnPoints[] points;
    [SerializeField] private float beforeStart = 1.25f;
    [SerializeField] private float betweenSpawns = 0.15f;
    [SerializeField] private GameObject coils;
    [SerializeField] private Animation pentogramAnim;

    internal void Attack()
    {
        pentogramAnim.Play();

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            StartCoroutine(SpawnCoils(index));
        }
    }

    IEnumerator SpawnCoils(int id)
    {
        float timer = beforeStart + (betweenSpawns * id);
        yield return new WaitForSeconds(timer);

        foreach (var point in points[id].point) { Instantiate(coils, point.position, Quaternion.identity); }
    }
}

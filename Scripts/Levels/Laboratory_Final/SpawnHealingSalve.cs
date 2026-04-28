using UnityEngine;

[System.Serializable]
public class SpawnZones
{
    public float maxX = 10;
    public float maxY = 10;
    public float minX = -10;
    public float minY = -10;
}

public class SpawnHealingSalve : MonoBehaviour
{
    [SerializeField] private GameObject healingSalve;
    [SerializeField] private SpawnZones[] spawnZones;

    private void Start() { SpawnSalve(); }

    public void SpawnSalve()
    {
        int randomZone = Random.Range(0, spawnZones.Length);
        float coordX = Random.Range(spawnZones[randomZone].minX, spawnZones[randomZone].maxX);
        float coordY = Random.Range(spawnZones[randomZone].minY, spawnZones[randomZone].maxY);
        Vector3 spawnPoint = new Vector3(coordX, coordY, 0f);

        Instantiate(healingSalve, spawnPoint, Quaternion.identity);
    }
}

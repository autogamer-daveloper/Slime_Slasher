using UnityEngine;

public class DeadLandsEffects : MonoBehaviour
{
    [Header("__ Player __")]
    [SerializeField] private Transform player;
    [SerializeField] private float maxX = 15f;
    [SerializeField] private float maxY = 7.5f;
    [Header("__ Effects __")]
    [SerializeField] private GameObject[] effects;
    [SerializeField] private float spawnTime = 0.75f;
    [SerializeField] private float startDelay = 0.2f;
    [SerializeField] private int startCount = 4;

    private int _startSpawned = 0;

    private Vector3 _left = new Vector3(1, 1, 1);
    private Vector3 _right = new Vector3(-1, 1, 1);

    private void Start() { StartCounting(); }

    private void StartCounting()
    {
        if (_startSpawned >= startCount) { return; }

        Invoke(nameof(StartCounting), startDelay);
        SpawnEffect();
        _startSpawned++;
    }

    private void SpawnEffect()
    {
        float minX = maxX * -1;
        float minY = maxY * -1;

        float coordX = Random.Range(minX, maxX);
        float coordY = Random.Range(minY, maxY);
        Vector3 coord = new Vector3(coordX + player.position.x, coordY + player.position.y, 0);

        int randomVector = Random.Range(0, 2);
        int randomEffect = Random.Range(0, effects.Length);

        Vector3 futureVector = new Vector3(0, 0, 0);
        if (randomVector == 0) { futureVector = _left; }
        else { futureVector = _right; }

        GameObject effect = Instantiate(effects[randomEffect], coord, player.rotation);
        effect.transform.localScale = futureVector;
        Invoke(nameof(SpawnEffect), spawnTime);
    }
}

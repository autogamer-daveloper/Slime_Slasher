using UnityEngine;
using System.Collections;

public class VillageMusicController : MonoBehaviour
{
    [Header("__ Village music type __")]
    [SerializeField] private GameObject[] musicTypes;

    private int _killedHooligans = 0;
    private int _hooligansCount = 3;

    private void Start() { StartCoroutine(SpawnMusic(0, 0.25f)); }

    IEnumerator SpawnMusic(int id, float timer)
    {
        foreach (GameObject obj in musicTypes) { obj.SetActive(false); }
        yield return new WaitForSeconds(timer);
        musicTypes[id].SetActive(true);
    }

    public void TargetedByHooligans() { StartCoroutine(SpawnMusic(1, 0.5f)); }

    public void KillHooligan()
    {
        _killedHooligans++;

        if(_killedHooligans >= _hooligansCount) { StartCoroutine(SpawnMusic(0, 0.25f)); }
    } 
}

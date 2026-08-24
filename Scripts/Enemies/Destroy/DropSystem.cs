using UnityEngine;

[System.Serializable]
public class DropType
{
    public GameObject item;
    public int chance = 50;    //типо число шанса, чем больше, тем больше вероятность получить дроп предмета
}

public class DropSystem : MonoBehaviour
{
    [Header("__ Is need custom spawn point? __")]
    [SerializeField] private bool isCustomSpawn = false;
    [SerializeField] private Transform targetPoint;
    [Header("__ Dropable items __")]
    [SerializeField] private DropType[] element;
    [Header("__ Extra settings __")]
    [SerializeField] private bool atStart = true;
    [SerializeField] private bool onlyOneItem = true;
    [SerializeField] private bool needDestroy = true;

    private bool _wasRewarded = false;

    private void Start() { if (atStart) { GiveResult(); } }

    public void GiveResult()
    {
        if (_wasRewarded) return;
        foreach (DropType item in element)
        {
            int _result = UnityEngine.Random.Range(0, 101);

            if (item.chance >= _result)
            {
                Transform target;
                if(isCustomSpawn) { target = targetPoint; }
                else { target = this.gameObject.transform; }

                Instantiate(item.item, target.position, this.gameObject.transform.rotation);
                Debug.Log($"Dropped item with result: {_result}");
                if (onlyOneItem)
                {
                    if (needDestroy) { _wasRewarded = true; Destroy(this.gameObject); }
                    return;
                }
            }
            else { Debug.Log($"Nothing dropped with result: {_result}"); }
        }
        
        if(needDestroy) { _wasRewarded = true; Destroy(this.gameObject); }
    }
}

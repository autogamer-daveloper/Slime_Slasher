using UnityEngine;

[System.Serializable]
public class DropType
{
    public GameObject item;
    public int chance = 50;    //типо число шанса, чем больше, тем больше вероятность получить дроп предмета
}

public class DropSystem : MonoBehaviour
{
    [Header("__ Dropable items __")]
    [SerializeField] private DropType[] element;
    [Header("__ Extra settings __")]
    [SerializeField] private bool atStart = true;

    private void Start()
    {
        if (atStart) { GiveResult(); }
    }

    public void GiveResult()
    {
        foreach (DropType item in element)
        {
            int _result = UnityEngine.Random.Range(0, 101);

            if (item.chance <= _result)
            {
                Instantiate(item.item, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Debug.Log($"Dropped item with result: {_result}");
            }
            else { Debug.Log($"Nothing dropped with result: {_result}"); }
        }

        Destroy(this.gameObject);
    }
}

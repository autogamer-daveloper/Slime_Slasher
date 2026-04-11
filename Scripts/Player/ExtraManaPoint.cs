using UnityEngine;

public class ExtraManaPoint : MonoBehaviour
{
    [Range(10, 100)]
    [SerializeField] private int countOfExtraMana = 35;

    internal int CountOfExtraMana() { return countOfExtraMana; }

    internal void DestroyThisPoint() { Destroy(this.gameObject); }
}

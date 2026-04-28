using UnityEngine;
using UnityEngine.Events;

public class ExtraManaPoint : MonoBehaviour
{
    [Range(10, 100)]
    [SerializeField] private int countOfExtraMana = 35;
    [Range(0, 200)]
    [SerializeField] private int countOfExtraLife = 0;
    [SerializeField] private bool isUsingAction = false;
    [SerializeField] private UnityEvent Destroying;

    internal int CountOfExtraMana() { return countOfExtraMana; }
    internal int CountOfExtraLife() { return countOfExtraLife; }

    internal void DestroyThisPoint() { if (isUsingAction) { Destroying.Invoke(); } Destroy(this.gameObject); }
}

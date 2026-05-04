using UnityEngine;
using UnityEngine.Events;

public class ThiefPlayer : MonoBehaviour
{
    [SerializeField] private UnityEvent Thief;
    public void DelayedThief() { Invoke(nameof(Steal), 20f); }
    private void Steal() { Thief.Invoke(); }
}

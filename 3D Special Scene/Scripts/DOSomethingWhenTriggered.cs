using UnityEngine;
using UnityEngine.Events;

public class DOSomethingWhenTriggered : MonoBehaviour
{
    [Space(20)]
    [Tooltip(" Do something when 3d player entered trigger ")]
    [SerializeField] private UnityEvent OnTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) { OnTriggered.Invoke(); }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ExtraTeleportToPoint : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private float waiting = 0.5f;
    [Space(20)]
    [SerializeField] private UnityEvent DoAfterTeleport;

    public void Teleport() { StartCoroutine(WaitBeforeTeleport(waiting)); }

    IEnumerator WaitBeforeTeleport(float time)
    {
        yield return new WaitForSeconds(time);
        _Teleport();
    }

    private void _Teleport()
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null) { cc.enabled = false; }

        player.position = target.position;

        if (cc != null) { cc.enabled = true; }
        DoAfterTeleport.Invoke();
    }
}

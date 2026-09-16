using UnityEngine;

public class KillPlayerWhileEscaping : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject killObject;

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { Instantiate(killObject, player.transform.position, Quaternion.identity); } }
}

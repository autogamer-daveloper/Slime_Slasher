using UnityEngine;

public class TeleportItem : MonoBehaviour
{
    [SerializeField] private Transform item;
    [SerializeField] private Transform target;

    public void Teleport_Item() { item.position = target.position; }
}

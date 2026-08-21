using UnityEngine;

public class PlayerBlockDamage : MonoBehaviour
{
    [SerializeField] private PlayerStatus status;

    private void OnEnable() { status.BlockDamage(true); }
    private void OnDisable() { status.BlockDamage(false); }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerSimpleTeleport : MonoBehaviour
{
    [Header("__ Teleport Animation __")]
    [SerializeField] private Animation anim;
    [Header("__ Teleport Button __")]
    [SerializeField] private Button teleportButton;
    [Header("__ Player __")]
    [SerializeField] private Transform player;
    [Header("__ Target __")]
    [SerializeField] private Transform target;

    private void Start() { teleportButton.onClick.AddListener(Teleport); }

    private void OnDestroy() { teleportButton.onClick.RemoveListener(Teleport); }

    private void Teleport()
    {
        anim.Play();
        Invoke(nameof(_Teleport), 0.5f);
    }

    private void _Teleport() { player.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0); }
}

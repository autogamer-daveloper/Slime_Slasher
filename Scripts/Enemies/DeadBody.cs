using UnityEngine;

public class DeadBody : MonoBehaviour
{
    [Header("__ Destroy Body __")]
    [SerializeField] private GameObject body;
    [SerializeField] private float delay = 15f;

    private void Start() { Invoke(nameof(Kill), delay); }

    private void Kill() { Destroy(body); }
}

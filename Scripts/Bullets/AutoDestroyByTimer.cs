using UnityEngine;

public class AutoDestroyByTimer : MonoBehaviour
{
    [Header("__ Timer __")]
    [SerializeField] private float timer = 1.5f;

    private void Start() { Invoke(nameof(Delete), timer); }

    private void Delete() { Destroy(this.gameObject); }
}

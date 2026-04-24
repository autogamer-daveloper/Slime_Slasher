using UnityEngine;
using Objects.Weapons;

public class BulletMovement : MonoBehaviour
{
    [Header("__ Combpleted attributes __")]
    [SerializeField] private Weapons weapon;
    [Header("__ Custom attributes __")]
    [SerializeField] private bool customProperties = true;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        if (customProperties == false)
        {
            speed = weapon.Speed;
            lifeTime = weapon.LifeTime;
        }

        Invoke(nameof(DestroyThis), lifeTime);
    }

    private void Update() { transform.Translate(Vector2.right * speed * Time.deltaTime); }

    internal void DestroyThis() { CancelInvoke(nameof(DestroyThis)); Destroy(this.gameObject); }
}

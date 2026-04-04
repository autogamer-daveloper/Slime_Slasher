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

        Invoke("DestroyThis", lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroyBullets"))
        {
            DestroyThis();
        }
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}

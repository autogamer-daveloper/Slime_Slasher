using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    [SerializeField] private BulletMovement bullet;

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("DestroyBullets")) { DestroyThisBullet(); } }

    private void DestroyThisBullet() { bullet.DestroyThis(); }
}

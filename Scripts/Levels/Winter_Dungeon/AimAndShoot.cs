using UnityEngine;

public class AimAndShoot : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject kunai;

    [Header("Rotation")]
    [Tooltip("Градусы, чтобы подогнать ориентацию спрайта (например, если спрайт 'смотрит' вверх, поставьте 90).")]
    [SerializeField] private float rotationOffset = 0f;
    [Tooltip("Скорость плавного поворота (градусы/сек). Если 0 — поворот будет мгновенным.")]
    [SerializeField] private float rotationSpeed = 360f;

    private void Update()
    {
        if (player != null)
        {
            Vector3 dir = player.position - this.gameObject.transform.position;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);

            if (rotationSpeed <= 0f)
            {
                this.gameObject.transform.rotation = targetRot;
            }
            else
            {
                float step = rotationSpeed * Time.deltaTime;
                this.gameObject.transform.rotation = Quaternion.RotateTowards(this.gameObject.transform.rotation, targetRot, step);
            }
        }
    }

    public void Shoot() { if(kunai != null) Instantiate(kunai, this.gameObject.transform.position, this.gameObject.transform.rotation); }
}

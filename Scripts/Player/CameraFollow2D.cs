using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;

    private void Start() { SnapToTarget(); }

    private void OnDestroy() { StopCoroutine(nameof(Snap)); }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    public void DelayedSnap(float delay)
    {
        StartCoroutine(Snap(delay));
    }

    IEnumerator Snap(float delay)
    {
        yield return new WaitForSeconds(delay);
        SnapToTarget();
    }

    private void SnapToTarget()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }
}
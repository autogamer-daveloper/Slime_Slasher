using UnityEngine;

public class EnemyCheckerFollow : MonoBehaviour
{
    [SerializeField] private Transform thisObj;
    [SerializeField] private Transform target;

    private void Update()
    {
        thisObj.position = target.position;
    }
}

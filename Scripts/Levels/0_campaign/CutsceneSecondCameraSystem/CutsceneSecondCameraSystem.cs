using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CutsceneSecondCameraSystem : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private CutsceneCameraPoint[] points;
    [SerializeField] private bool atStart = false;
    [Header("__ Cameras __")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject cutsceneCamera;
    [SerializeField] private Transform cutsceneCameraTransform;
    [SerializeField] private bool needEnd = false;

    private void Start() { if (atStart) { StartMoving(); } }

    public void StartCutsceneCamera() { StartMoving(); }

    private void StartMoving()
    {
        playerCamera.SetActive(false);
        cutsceneCamera.SetActive(true);

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            StartCoroutine(CallMoving(index, points[index].timeToActivate));
        }
    }

    IEnumerator CallMoving(int id, float timer)
    {
        yield return new WaitForSeconds(timer);
        MoveCameraToPoint(id);
    }

    private void MoveCameraToPoint(int id)
    {
        Vector3 result = new Vector3(points[id].point.position.x, points[id].point.position.y, -10);
        cutsceneCameraTransform.DOMove(result, points[id].timeToMove);

        if (id >= points.Length - 1 && needEnd) { Invoke(nameof(EndedCutscene), points[id].timeToMove); }
    }

    private void EndedCutscene()
    {
        playerCamera.SetActive(true);
        cutsceneCamera.SetActive(false);
    }
}

[System.Serializable]
internal class CutsceneCameraPoint
{
    public Transform point;
    public float timeToMove = 5f;
    public float timeToActivate = 0f;
}
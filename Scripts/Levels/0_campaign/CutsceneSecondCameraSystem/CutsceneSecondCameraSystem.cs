using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private void Start() { if (atStart) { StartMoving(); } }

    public void StartCutsceneCamera() { StartMoving(); }

    private void StartMoving()
    {
        playerCamera.SetActive(false);
        cutsceneCamera.SetActive(true);

        activeCoroutines.Clear();

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            Coroutine coroutine = StartCoroutine(CallMoving(index, points[index].timeToActivate));
            activeCoroutines.Add(coroutine);
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

    public void CancelCutsceneCamera()
    {
        CancelInvoke(nameof(EndedCutscene));

        foreach (Coroutine coroutine in activeCoroutines) { if (coroutine != null) { StopCoroutine(coroutine); } }

        activeCoroutines.Clear();

        EndedCutscene();
    }

    internal void RecalculateCameraWaypoint(int id)
    {
        StopAllCoroutines();
        activeCoroutines.Clear();

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            if (points[index].timeToActivate >= points[id].timeToActivate)
            {
                Coroutine coroutine = StartCoroutine(CallMoving(index, points[index].timeToActivate - points[id].timeToActivate));
                activeCoroutines.Add(coroutine);   
            }
        }
    }
}

[System.Serializable]
internal class CutsceneCameraPoint
{
    public Transform point;
    public float timeToMove = 5f;
    public float timeToActivate = 0f;
}
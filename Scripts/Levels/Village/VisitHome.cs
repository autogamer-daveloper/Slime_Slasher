using UnityEngine;
using UnityEngine.Events;

public class VisitHome : MonoBehaviour
{
    [SerializeField] private GameObject[] activate;
    [SerializeField] private UnityEvent AfterVisitingHome;

    private bool isVisited = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isVisited) return;

        isVisited = true;
        AfterVisitingHome.Invoke();
        KeyManager.Set_Bool_Key("VisitedHome", 1);

        foreach (GameObject obj in activate)
        {
            obj.SetActive(true);
        }
    }
}

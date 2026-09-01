using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Sportiki : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private string keyName = "isSlimePrisonerKilled";
    [Header("__ If prisoner killed __")]
    [SerializeField] private GameObject[] ifKilled;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject[] realKillers;
    [SerializeField] private Button spawnButton;
    [Header("__ Any __")]
    [SerializeField] private GameObject Jo;
    [SerializeField] private GameObject[] any;
    [SerializeField] private GameObject[] anyDestroy;
    [Header("__ Actions __")]
    [Tooltip("What to do when leave house you will meet hooligans.")]
    [Space(5)]
    [SerializeField] private UnityEvent withHooligans;
    [Tooltip("What to do when leave house you won't meet hooligans.")]
    [Space(5)]
    [SerializeField] private UnityEvent withoutHooligans;

    private int isKilledPrisoner = 0;
    private int wasAtHome = 0;

    private void Start()
    {
        isKilledPrisoner = KeyManager.Get_Bool_Key(keyName);

        wasAtHome = KeyManager.Get_Bool_Key("VisitedHome");
        if (wasAtHome == 1) { WasAtHome(); }

        spawnButton.onClick.AddListener(InvokeReal);
    }

    private void OnDestroy() { spawnButton.onClick.RemoveListener(InvokeReal); }

    public void InvokeSportikov()
    {
        if (isKilledPrisoner == 1) { Killed(); }
        else { NotKilled(); }
    }

    private void WasAtHome()
    {
        foreach (GameObject obj in any) { obj.SetActive(true); }
        foreach (GameObject obj in anyDestroy) { obj.SetActive(false); }
    }

    private void Killed()
    {
        foreach (GameObject obj in any) { obj.SetActive(true); }
        foreach (GameObject obj in anyDestroy) { obj.SetActive(false); }

        Jo.SetActive(true);

        withoutHooligans.Invoke();
    }

    private void NotKilled()
    {
        foreach (GameObject obj in any) { obj.SetActive(true); }
        foreach (GameObject obj in anyDestroy) { obj.SetActive(false); }

        if (wasAtHome == 0)
        {
            Jo.SetActive(true);

            foreach (GameObject obj in ifKilled) { obj.SetActive(true); }
        }

        withHooligans.Invoke();
    }

    private void InvokeReal()
    {
        if (isKilledPrisoner == 0 && wasAtHome == 0)
        {
            dialogue.SetActive(true);
        }
    }

    public void SpawnRealKillers()
    {
        foreach (GameObject obj in ifKilled) { obj.SetActive(false); }
        foreach (GameObject obj in realKillers) { obj.SetActive(true); }
    }
}

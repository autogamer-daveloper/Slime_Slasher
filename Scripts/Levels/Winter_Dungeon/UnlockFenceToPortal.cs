using UnityEngine;
using UnityEngine.UI;

public class UnlockFenceToPortal : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private SkeletonLordBossFight boss;
    [SerializeField] private GameObject opened;
    [SerializeField] private GameObject closed;
    [Header("__ Help __")]
    [SerializeField] private GameObject help;
    [SerializeField] private Animation helpAnim;
    [Header("__ UI __")]
    [SerializeField] private Button getHelp;

    private string key;

    private void Start()
    {
        key = boss.GetBossKey();
        Check();

        getHelp.onClick.AddListener(GetHelp);
    }

    private void OnDestroy() { getHelp.onClick.RemoveListener(GetHelp); }

    public void Check()
    {
        int result = KeyManager.Get_Bool_Key(key);
        if (result == 1) { opened.SetActive(true); closed.SetActive(false); }
        else { opened.SetActive(false); closed.SetActive(true); }
    }

    private void GetHelp()
    {
        help.SetActive(true);
        helpAnim.Play();
    }
}

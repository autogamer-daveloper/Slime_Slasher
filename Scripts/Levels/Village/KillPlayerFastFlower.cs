using UnityEngine;
using UnityEngine.UI;

public class KillPlayerFastFlower : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private Transform player;
    [SerializeField] private Button[] kill;
    [SerializeField] private GameObject killingObject;
    [SerializeField] private DenseTreeLogic logic;
    [SerializeField] private GameObject deactivate;
    [SerializeField] private GameObject activate;

    private string bossKey;

    private void Start()
    {
        bossKey = logic.GetBossKey();
        foreach (Button btn in kill)
        {
            btn.onClick.AddListener(Kill);
        }

        Check();
    }

    [ContextMenu("Check")]
    public void Check()
    {
        int hasKey = KeyManager.Get_Bool_Key(bossKey);
        if (hasKey == 1)
        {
            deactivate.SetActive(false);
            activate.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        foreach (Button btn in kill)
        {
            btn.onClick.RemoveListener(Kill);
        }
    }

    private void Kill()
    {
        int hasKey = KeyManager.Get_Bool_Key(bossKey);
        if (hasKey == 0)
        {
            Instantiate(killingObject, player.position, player.rotation);
        }
    }
}

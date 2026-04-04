using UnityEngine;
using UnityEngine.UI;

public class IronGate : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private GameObject Opened;
    [SerializeField] private GameObject Closed;
    [SerializeField] private Button unlock;
    [SerializeField] private string key = "ironGateOpened";

    private int isOpened = 0;

    private void Start()
    {
        isOpened = KeyManager.Get_Bool_Key(key);
        if (isOpened == 1)
        {
            UnlockedGate();
        }
        else
        {
            LockedGate();
        }

        unlock.onClick.AddListener(Unlock);
    }

    private void OnDestroy()
    {
        unlock.onClick.RemoveListener(Unlock);
    }

    private void Unlock()
    {
        int _key = KeyManager.Get_Item_Count(36);
        if (_key >= 1)
        {
            isOpened = 1;
            KeyManager.Spend_Item(36, _key);
            UnlockedGate();
            KeyManager.Set_Bool_Key(key, 1);
        }
    }

    private void UnlockedGate()
    {
        Opened.SetActive(true);
        Closed.SetActive(false);
    }

    private void LockedGate()
    {
        Opened.SetActive(false);
        Closed.SetActive(true);
    }
}

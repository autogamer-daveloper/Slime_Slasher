using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    [SerializeField] private GameObject warning;

    private void Start()
    {
        int i = KeyManager.Get_Bool_Key("WarningShowed");
        if (i == 1) { warning.SetActive(false); }
        else { warning.SetActive(true); }
        KeyManager.Set_Bool_Key("WarningShowed", 0);
    }
}

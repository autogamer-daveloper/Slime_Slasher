using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject rateGame;

    private void Start()
    {
        int x = KeyManager.Get_Bool_Key("WarningShowed");
        int u = KeyManager.Get_Bool_Key("Rated");
        int i = KeyManager.Get_Bool_Key("IsRatedShowed");
        if (x == 1)
        {
            warning.SetActive(false);
            if (u != 1 && i == 0) { rateGame.SetActive(true); }
        }
        else { warning.SetActive(true); KeyManager.Set_Bool_Key("IsWarningShowed", 0); }
        KeyManager.Set_Bool_Key("WarningShowed", 0);
    }

    public void ShowedRatePanel() { KeyManager.Set_Bool_Key("IsWarningShowed", 1); }
}

using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    [Header("__ Input type __")]
    [SerializeField] private InputType inputType;

    [Header("__ UI: Panels __")]
    [Tooltip("Warning panel, shows when game start.")]
    [SerializeField] private GameObject warning;
    [Tooltip("Rate panel, shows when warning panel is showed, but rate panel isn't.")]
    [SerializeField] private GameObject rateGame;

    private const string showWarningKey = "IsShowWarning";

    private void Start()
    {
        bool isMobile = inputType.IsMobileInput();
        int x = KeyManager.Get_Bool_Key("WarningShowed");
        int u = KeyManager.Get_Bool_Key("Rated");
        int i = KeyManager.Get_Bool_Key("IsRatedShowed");
        if (x == 1)
        {
            warning.SetActive(false);
            if (u != 1 && i == 0 && isMobile) { rateGame.SetActive(true); }
        }
        else { TryToShowWarning(); }
        KeyManager.Set_Bool_Key("WarningShowed", 0);
    }

    public void ShowedRatePanel() { KeyManager.Set_Bool_Key("IsWarningShowed", 1); }

    private void TryToShowWarning()
    {
        if (PlayerPrefs.HasKey(showWarningKey))
        {
            int isWeCanShowWarn = KeyManager.Get_Bool_Key(showWarningKey);
            if (isWeCanShowWarn == 1) { ShowWarning(); }
        } else { ShowWarning(); }
    }

    private void ShowWarning()
    {
        warning.SetActive(true);
        KeyManager.Set_Bool_Key("IsWarningShowed", 0);
    }
}

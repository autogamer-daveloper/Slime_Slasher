using UnityEngine;

public class VillageHelper : MonoBehaviour
{
    [Header("__ Help: Go to home __")]
    [SerializeField] private GameObject helpGoHome;
    [SerializeField] private Animation helpGoHomeAnim;
    [SerializeField] private string _foundSon = "SonWasFound";

    [Header("__ Help: Find tawern __")]
    [SerializeField] private GameObject helpTawern;
    [SerializeField] private Animation helpTawernAnim;
    [SerializeField] private string _foundTawern = "TawernWasFound";

    [Header("__ Help: Trade __")]
    [SerializeField] private GameObject helpTrade;
    [SerializeField] private Animation helpTradeAnim;
    [SerializeField] private string _foundKey = "KeyWasFound";

    [Header("__ Help: Find boss __")]
    [SerializeField] private GameObject helpBoss;
    [SerializeField] private Animation helpBossAnim;
    [SerializeField] private string _foundBoss = "BossWasFound";

    [Header("__ Help: Last help __")]
    [SerializeField] private GameObject helpTeleport;
    [SerializeField] private Animation helpTeleportAnim;
    [SerializeField] private string _defeatedBoss = "isDenseTreeDefeated";

    private void Start()
    {
        int foundSon = KeyManager.Get_Bool_Key(_foundSon);
        int foundTawern = KeyManager.Get_Bool_Key(_foundTawern);
        int foundKey = KeyManager.Get_Bool_Key(_foundKey);
        int foundBoss = KeyManager.Get_Bool_Key(_foundBoss);
        int defeatedBoss = KeyManager.Get_Bool_Key(_defeatedBoss);

        if (defeatedBoss == 1) { ShowHint(4); return; }
        if (foundSon == 0) { ShowHint(0); return; }
        if (foundTawern == 0) { ShowHint(1); return; }
        if (foundKey == 0) { ShowHint(2); return; }
        if (foundBoss == 0) { ShowHint(3); return; }
    }

    private void ShowHint(int id)
    {
        switch (id)
        {
            case 0: helpGoHome.SetActive(true); helpGoHomeAnim.Play(); break;
            case 1: helpTawern.SetActive(true); helpTawernAnim.Play(); break;
            case 2: helpTrade.SetActive(true); helpTradeAnim.Play(); break;
            case 3: helpBoss.SetActive(true); helpBossAnim.Play(); break;
            case 4: helpTeleport.SetActive(true); helpTeleportAnim.Play(); break;
        }
    }

    public void Get_Home() { KeyManager.Set_Bool_Key(_foundSon, 1); ShowHint(1); }
    public void Get_Tawern() { KeyManager.Set_Bool_Key(_foundTawern, 1); ShowHint(2); }
    public void Get_Key() { KeyManager.Set_Bool_Key(_foundKey, 1); ShowHint(3); }
    public void Get_Boss() { KeyManager.Set_Bool_Key(_foundBoss, 1); ShowHint(4); }
}

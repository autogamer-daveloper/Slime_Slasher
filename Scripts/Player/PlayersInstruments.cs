using UnityEngine;
using UnityEngine.UI;

public class PlayersInstruments : MonoBehaviour
{
    [Header("__ Instruments: Pickaxe __")]
    [SerializeField] private Button stonePickaxe;
    [SerializeField] private Button ironPickaxe;
    [SerializeField] private Button goldenPickaxe;
    [Header("__ Instruments: Axe __")]
    [SerializeField] private Button ironAxe;
    [SerializeField] private Button goldenAxe;

    private void Start()
    {
        SerializeButtons();
    }

    private void SerializeButtons()
    {
        //pickaxe
        stonePickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(1); });
        ironPickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(2); });
        goldenPickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(3); });
        //axe
        ironAxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Axe(2); });
        goldenAxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Axe(3); });
    }

    private void OnDestroy()
    {
        //pickaxe
        stonePickaxe.onClick.RemoveAllListeners();
        ironPickaxe.onClick.RemoveAllListeners();
        goldenPickaxe.onClick.RemoveAllListeners();
        //axe
        ironAxe.onClick.RemoveAllListeners();
        goldenAxe.onClick.RemoveAllListeners();
    }
}

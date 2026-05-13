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
    [Header("__ Audio Source __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip selectItem;

    private void Start() { SerializeButtons(); }

    private void SerializeButtons()
    {
        //pickaxe
        stonePickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(1); src.PlayOneShot(selectItem); });
        ironPickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(2); src.PlayOneShot(selectItem); });
        goldenPickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(3); src.PlayOneShot(selectItem); });
        //axe
        ironAxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Axe(2); src.PlayOneShot(selectItem); });
        goldenAxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Axe(3); src.PlayOneShot(selectItem); });
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

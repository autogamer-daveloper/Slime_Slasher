using UnityEngine;
using UnityEngine.UI;

public class PlayersInstruments : MonoBehaviour
{
    [Header("__ Instruments: Pickaxe __")]
    [SerializeField] private Button stonePickaxe;
    [SerializeField] private Button ironPickaxe;
    [SerializeField] private Button goldenPickaxe;
    [SerializeField] private GameObject[] pickaxePictures;
    [Header("__ Instruments: Axe __")]
    [SerializeField] private Button ironAxe;
    [SerializeField] private Button goldenAxe;
    [SerializeField] private GameObject[] axePictures;
    [Header("__ Audio Source __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip selectItem;

    private void Start() { SerializeButtons(); }

    private void SerializeButtons()
    {
        //pickaxe
        stonePickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(1); src.PlayOneShot(selectItem); SelectedPickaxe(1); });
        ironPickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(2); src.PlayOneShot(selectItem); SelectedPickaxe(2); });
        goldenPickaxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Pickaxe(3); src.PlayOneShot(selectItem); SelectedPickaxe(3); });
        //axe
        ironAxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Axe(2); src.PlayOneShot(selectItem); SelectedAxe(1); });
        goldenAxe.onClick.AddListener(() => { KeyManager.SetInt_InstrumentPower_Axe(3); src.PlayOneShot(selectItem); SelectedAxe(2); });
    }

    private void SelectedPickaxe(int id) { foreach (GameObject obj in pickaxePictures) { obj.SetActive(false); } pickaxePictures[id].SetActive(true); }
    private void SelectedAxe(int id) { foreach (GameObject obj in axePictures) { obj.SetActive(false); } axePictures[id].SetActive(true); }

    public void SelectedPickaxePublic(int id) { SelectedPickaxe(id); }
    public void SelectedAxePublic(int id) { SelectedAxe(id); }

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

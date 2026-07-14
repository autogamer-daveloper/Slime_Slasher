using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class VisualAccessoryProduct
{
    [Header(" Main settings ")]
    public int visualId = 0; // counting from 1
    public string boughtKey = "visual_bought_0";
    public Button use;
    public GameObject usingDot;
    [Space(10)]
    [Header(" Shop settings, if can be bought ")]
    public bool canBuy = true;
    public int price = 1200;
    public TMP_Text priceText;
    public Button buy;
    public GameObject buyButton;
}

public class VisualAccessoriesShop : MonoBehaviour
{
    [Header("__ Products __")]
    [SerializeField] private VisualAccessoryProduct[] products;
    [Header("__ Extra UI __")]
    [SerializeField] private Button removeVisualAccessories;
    [SerializeField] private TMP_Text astraslimesText;
    [SerializeField] private GameObject helpNotEnought;
    [SerializeField] private Animation helpNotEnoughtAnim;

    private int _astraslimes = 0;

    private void Start() { _astraslimes = KeyManager.Get_Bool_Key("Astraslimes"); Initialize(); UpdateUI(); removeVisualAccessories.onClick.AddListener(DeleteVisualAccessories); }

    private void OnDestroy() { removeVisualAccessories.onClick.RemoveListener(DeleteVisualAccessories); }

    private void UpdateUI() { Debug.LogWarning("|Updated UI|"); astraslimesText.text = _astraslimes.ToString(); }

    private void Initialize()
    {
        Debug.LogWarning("|Initializing visual accessories|");
        for (int i = 0; i < products.Length; i++)
        {
            int index = i;
            int isBought = KeyManager.Get_Bool_Key(products[index].boughtKey);
            if (products[index].canBuy)
            {
                if (isBought != 0) { products[index].buyButton.SetActive(false); }
                else { products[index].buyButton.SetActive(true); }
                products[index].priceText.text = products[index].price.ToString();
                products[index].buy.onClick.RemoveAllListeners();
                products[index].buy.onClick.AddListener(() => { BuyVisualAccessory(index); });
            }
            int isUsing = KeyManager.GetInt_VisualAccessoryID();
            if (isUsing == products[index].visualId) { products[index].usingDot.SetActive(true); }
            else { products[index].usingDot.SetActive(false); }
            products[index].use.onClick.RemoveAllListeners();
            products[index].use.onClick.AddListener(() => { UseVisualAccessory(index); });
            if (isBought != 0) { products[index].use.interactable = true; }
            else { products[index].use.interactable = false; }
            Debug.LogWarning($"|Initialized {index} visual accessory|");
        }
    }

    private void BuyVisualAccessory(int id)
    {
        Debug.LogWarning($"|Buying {id} visual accessory|");
        if (_astraslimes >= products[id].price)
        {
            Debug.LogWarning($"|Bought {id} visual accessory|");
            KeyManager.Set_Bool_Key(products[id].boughtKey, 1);
            _astraslimes -= products[id].price;
            KeyManager.Set_Bool_Key("Astraslimes", _astraslimes);
        }
        else
        {
            helpNotEnought.SetActive(true);
            helpNotEnoughtAnim.Play();
        }

        Initialize(); UpdateUI();
    }

    private void UseVisualAccessory(int id)
    {
        Debug.LogWarning($"|Using {id} visual accessory|");
        int isBought = KeyManager.Get_Bool_Key(products[id].boughtKey);
        if (isBought != 0) { KeyManager.SetInt_VisualAccessoryID(products[id].visualId); }
        Initialize(); UpdateUI();
    }

    private void DeleteVisualAccessories()
    {
        Debug.LogWarning("|Deleted visual accessories|");
        KeyManager.SetInt_VisualAccessoryID(0);
        Initialize(); UpdateUI();
    }

    public void ReInitialize() { Initialize(); }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class VisualAccessoryProduct
{
    [Header(" Main settings ")]
    [Tooltip("Accessory id. Need to be unique. Counting from 1.")]
    public int visualId = 0; // counting from 1
    [Tooltip("Accessory key. Need to be unique. May use key like 'visual_bought_(visualId)'.")]
    public string boughtKey = "visual_bought_0";
    [Tooltip("Button, on click which you will select this accessory.")]
    public Button use;
    /*[Tooltip("Temporary! Shows what accessory was selected.")]
    public GameObject usingDot; */
    [Tooltip("Shows what accessory was selected.")]
    public Image ramkImage;
    [Space(10)]
    [Header(" Shop settings, if can be bought ")]
    [Tooltip("Is this accessory can be bought?")]
    public bool canBuy = true;
    [Tooltip("If you can buy it, which it cost for player?")]
    public int price = 1200;
    [Tooltip("If you can buy it, where price will shown?")]
    public TMP_Text priceText;
    [Tooltip("If you can buy, with which button player will buy this accessory&")]
    public Button buy;
    [Tooltip("Same object that was in field 'buy'.")]
    public GameObject buyButton;
}

public class VisualAccessoriesShop : MonoBehaviour
{
    [Header("__ Products __")]
    [Tooltip("Set up your accessories.")]
    [SerializeField] private VisualAccessoryProduct[] products;
    [Header("__ Extra UI __")]
    [Tooltip("Button, on click which remove all accessories.")]
    [SerializeField] private Button removeVisualAccessories;
    [Tooltip("Astraslimes (vallet) text. Show value of this vallet.")]
    [SerializeField] private TMP_Text astraslimesText;
    [Tooltip("Help tip object.")]
    [SerializeField] private GameObject helpNotEnought;
    [Tooltip("Help tip animation component.")]
    [SerializeField] private Animation helpNotEnoughtAnim;
    [Tooltip("In active ramk sprite.")]
    [SerializeField] private Sprite inactiveSlot;
    [Tooltip("Active ramk sprite.")]
    [SerializeField] private Sprite activeSlot;

    private int _astraslimes = 0;

    private void Start() { _astraslimes = KeyManager.Get_Bool_Key("Astraslimes"); Initialize(); UpdateUI(); removeVisualAccessories.onClick.AddListener(DeleteVisualAccessories); }

    private void OnDestroy() { removeVisualAccessories.onClick.RemoveListener(DeleteVisualAccessories); }

    private void UpdateUI() { Debug.Log("|Updated UI|"); astraslimesText.text = _astraslimes.ToString(); }

    private void Initialize()
    {
        Debug.Log("|Initializing visual accessories|");
        for (int i = 0; i < products.Length; i++)
        {
            int index = i;
            int isBought = KeyManager.Get_Bool_Key(products[index].boughtKey);
            if (products[index].buy != null)
            {
                if (isBought != 0) { products[index].buy.gameObject.SetActive(false); }
                else { products[index].buy.gameObject.SetActive(true); }
            }
            if (products[index].canBuy)
            {
                products[index].priceText.text = products[index].price.ToString();
                products[index].buy.onClick.RemoveAllListeners();
                products[index].buy.onClick.AddListener(() => { BuyVisualAccessory(index); });
            }
            int isUsing = KeyManager.GetInt_VisualAccessoryID();
            if (isUsing == products[index].visualId) { /*products[index].usingDot.SetActive(true);*/ products[index].ramkImage.sprite = activeSlot; }
            else { /*products[index].usingDot.SetActive(false);*/ products[index].ramkImage.sprite = inactiveSlot; }
            products[index].use.onClick.RemoveAllListeners();
            products[index].use.onClick.AddListener(() => { UseVisualAccessory(index); });
            if (isBought != 0) { products[index].use.interactable = true; }
            else { products[index].use.interactable = false; }
            Debug.Log($"|Initialized {index} visual accessory|");
        }
    }

    public void GetForFreeVisualAccessory(int id)
    {
        KeyManager.Set_Bool_Key(products[id].boughtKey, 1);
        Initialize(); UpdateUI();
    }

    private void BuyVisualAccessory(int id)
    {
        Debug.Log($"|Buying {id} visual accessory|");
        if (_astraslimes >= products[id].price)
        {
            Debug.Log($"|Bought {id} visual accessory|");
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
        Debug.Log($"|Using {id} visual accessory|");
        int isBought = KeyManager.Get_Bool_Key(products[id].boughtKey);
        if (isBought != 0) { KeyManager.SetInt_VisualAccessoryID(products[id].visualId); }
        Initialize(); UpdateUI();
    }

    private void DeleteVisualAccessories()
    {
        Debug.Log("|Deleted visual accessories|");
        KeyManager.SetInt_VisualAccessoryID(0);
        Initialize(); UpdateUI();
    }

    public void ReInitialize() { Initialize(); }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

[System.Serializable]
public class Trade
{
    public Button acceptTrade;
    public int[] itemsId;
    public int[] needItems;
    public TMP_Text[] countItems;
    public UnityEvent whatToDo;
    public bool isOnceTradeOffer = false;
    public string tradeOfferName = "trade_offer_axe";
    public GameObject traded;
}

[System.Serializable]
public class Craft
{
    public Button acceptCraft;
    public int[] itemsId;
    public int[] needItems;
    public TMP_Text[] countItems;
    public UnityEvent whatToDo;
    public int receivedItemId = 1;
    public int receivedItemCount = 1;
    public bool isOnceOffer = false;
    public string offerName = "craft_offer_";
    public GameObject crafted;
}

public class CraftOrTradingSystem : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private Button[] switchPanel;
    [SerializeField] private RectTransform panel;
    [SerializeField] private float speed = 0.5f;
    [Header("__ Trading __")]
    [SerializeField] private Trade[] trades;
    [Header("__ Crafting __")]
    [SerializeField] private Craft[] crafts;
    [Header("__ Inventory __")]
    [SerializeField] private Inventory inv;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip whoosh;
    [SerializeField] private AudioClip button;
    [SerializeField] private AudioClip getInv;

    private bool _isShown = false;

    private void Start()
    {
        foreach (Button btn in switchPanel) { btn.onClick.AddListener(SwitchPanel); }

        if (trades != null)
        {
            for (int i = 0; i < trades.Length; i++)
            {
                int id = i;
                var t = trades[id];
                if (t == null) continue;
                if (t.acceptTrade == null)
                {
                    Debug.LogWarning($"[CraftOrTradingSystem] trades[{i}].acceptTrade == null");
                    continue;
                }
                t.acceptTrade.onClick.AddListener(() => TradeItem(id));
                if (t.isOnceTradeOffer)
                {
                    int isTraded = KeyManager.Get_Bool_Key(t.tradeOfferName);
                    if (isTraded != 0)
                    {
                        t.traded.SetActive(true);
                        t.acceptTrade.interactable = false;
                    }
                }
            }
        }

        if (crafts != null)
        {
            for (int i = 0; i < crafts.Length; i++)
            {
                int id = i;
                var c = crafts[id];
                if (c == null) continue;
                if (c.acceptCraft == null)
                {
                    Debug.LogWarning($"[CraftOrTradingSystem] crafts[{i}].acceptCraft == null");
                    continue;
                }
                c.acceptCraft.onClick.AddListener(() => CraftItem(id));
                if (c.isOnceOffer)
                {
                    int isCrafted = KeyManager.Get_Bool_Key(c.offerName);
                    if (isCrafted != 0)
                    {
                        c.crafted.SetActive(true);
                        c.acceptCraft.interactable = false;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Button btn in switchPanel) { btn.onClick.RemoveAllListeners(); }

        if (trades != null)
        {
            for (int i = 0; i < trades.Length; i++)
            {
                var t = trades[i];
                if (t == null || t.acceptTrade == null) continue;
                t.acceptTrade.onClick.RemoveAllListeners();
            }
        }

        if (crafts != null)
        {
            for (int i = 0; i < crafts.Length; i++)
            {
                var c = crafts[i];
                if (c == null || c.acceptCraft == null) continue;
                c.acceptCraft.onClick.RemoveAllListeners();
            }
        }
    }

    private void SwitchPanel()
    {
        if (_isShown == true)
        {
            Vector2 hidden = new Vector2(0, -2000);
            panel.DOAnchorPos(hidden, speed);
        }
        else
        {
            Vector2 shown = new Vector2(0, 0);
            panel.DOAnchorPos(shown, speed);
        }

        src.PlayOneShot(button);
        src.PlayOneShot(whoosh);
        _isShown = !_isShown;
        ShowPlayersItemCount();
    }

    private void ShowPlayersItemCount()
    {
        for (int i = 0; i < trades.Length; i++)
        {
            int id = i;
            for (int x = 0; x < trades[id].itemsId.Length; x++)
            {
                int item = x;
                int itemCount = KeyManager.Get_Item_Count(trades[id].itemsId[item]);
                trades[id].countItems[item].text = itemCount.ToString() + "/" + trades[id].needItems[item];
            }
        }

        for (int i = 0; i < crafts.Length; i++)
        {
            int id = i;
            for (int x = 0; x < crafts[id].itemsId.Length; x++)
            {
                int item = x;
                int itemCount = KeyManager.Get_Item_Count(crafts[id].itemsId[item]);
                crafts[id].countItems[item].text = itemCount.ToString() + "/" + crafts[id].needItems[item];
            }
        }
    }

    private bool CanTrade(int tradeId)
    {
        int needToBeTraded = trades[tradeId].itemsId.Length;
        int tradableItems = 0;

        for (int x = 0; x < trades[tradeId].itemsId.Length; x++)
        {
            int item = x;
            int itemCount = KeyManager.Get_Item_Count(trades[tradeId].itemsId[item]);
            if (itemCount >= trades[tradeId].needItems[item]) { tradableItems++; }
        }

        if (tradableItems >= needToBeTraded) { return true; }
        else { return false; }
    }

    private bool CanCraft(int craftId)
    {
        int needToBeCrafted = crafts[craftId].itemsId.Length;
        int craftableItems = 0;

        for (int x = 0; x < crafts[craftId].itemsId.Length; x++)
        {
            int item = x;
            int itemCount = KeyManager.Get_Item_Count(crafts[craftId].itemsId[item]);
            if (itemCount >= crafts[craftId].needItems[item]) { craftableItems++; }
        }

        if (craftableItems >= needToBeCrafted) { return true; }
        else { return false; }
    }

    private void TradeItem(int tradeId)
    {
        Debug.Log($"Trade requested id={tradeId}");

        bool answer = CanTrade(tradeId);
        if (answer == true)
        {
            for (int i = 0; i < trades[tradeId].itemsId.Length; i++)
            {
                int id = i;
                KeyManager.Spend_Item(trades[tradeId].itemsId[id], trades[tradeId].needItems[id]);
            }

            if (trades[tradeId].isOnceTradeOffer)
            {
                KeyManager.Set_Bool_Key(trades[tradeId].tradeOfferName, 1);
                trades[tradeId].traded.SetActive(true);
                trades[tradeId].acceptTrade.interactable = false;
            }

            trades[tradeId].whatToDo.Invoke();
            inv.Refresh();
            src.PlayOneShot(button);
        }
        else { Debug.Log($"[CraftOrTradingSystem {gameObject.name}]: can't trade items {tradeId}. No enought items"); }

        ShowPlayersItemCount();
    }

    private void CraftItem(int craftId)
    {
        Debug.Log($"Craft requested id={craftId}");

        bool answer = CanCraft(craftId);
        if (answer == true)
        {
            for (int i = 0; i < crafts[craftId].itemsId.Length; i++)
            {
                int id = i;
                KeyManager.Spend_Item(crafts[craftId].itemsId[id], crafts[craftId].needItems[id]);
            }

            if (crafts[craftId].isOnceOffer)
            {
                KeyManager.Set_Bool_Key(crafts[craftId].offerName, 1);
                crafts[craftId].crafted.SetActive(true);
                crafts[craftId].acceptCraft.interactable = false;
            }

            crafts[craftId].whatToDo.Invoke();
            KeyManager.Receive_Item(crafts[craftId].receivedItemId, crafts[craftId].receivedItemCount);
            inv.Refresh();
            src.PlayOneShot(button);
        }
        else { Debug.Log($"[CraftOrTradingSystem {gameObject.name}]: can't craft item {craftId}. No enought items"); }

        ShowPlayersItemCount();
    }

    public void GetItemOnce(int id)
    {
        KeyManager.Receive_Item(id, 1);
        src.PlayOneShot(getInv);
    }
}
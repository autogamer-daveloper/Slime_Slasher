using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public enum InstrumentType
{
    None,
    Axe,
    Pickaxe
}

[System.Serializable]
public class GameElement
{
    public Button button;
    public Button receive;
    public int countGet = 1;
    public Image image;
    public bool countable = false;
    public TMP_Text count;
    public int itemId = 0;
    public InstrumentType type = InstrumentType.None;
    public int needPower = 0;
    public AudioClip receiveClip;
}

public class Inventory : MonoBehaviour
{
    [Header("__ Main __")]
    [SerializeField] private Color interactable;
    [SerializeField] private Color no_interactable;
    [Header("__ Items __")]
    public GameElement[] elements;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;

    private UnityAction[] _receiveActions;
    private UnityAction[] _soundActions;

    internal void Refresh() { SerializeItems(); }

    private void Start()
    {
        KeyManager.Receive_Item_Once(0);
        SerializeReceiveButtons();
        SerializeItems();
    }

    internal void SerializeReceiveButtons()
    {
        if (elements == null) return;

        if (_receiveActions == null || _receiveActions.Length != elements.Length) { _receiveActions = new UnityAction[elements.Length]; }

        if (_soundActions == null || _soundActions.Length != elements.Length) { _soundActions = new UnityAction[elements.Length]; }

        for (int i = 0; i < elements.Length; i++)
        {
            var el = elements[i];
            if (el == null || el.receive == null) continue;

            if (_receiveActions[i] != null) { el.receive.onClick.RemoveListener(_receiveActions[i]); }

            if (_soundActions[i] != null) { el.receive.onClick.RemoveListener(_soundActions[i]); }

            int id = i;
            //int id = el.itemId;

            UnityAction itemAction = () => GetItem(id);
            _receiveActions[i] = itemAction;
            el.receive.onClick.AddListener(itemAction);

            UnityAction soundAction = () => { if (src != null && el.receiveClip != null) { src.PlayOneShot(el.receiveClip); }};
            _soundActions[i] = soundAction;
            el.receive.onClick.AddListener(soundAction);
        }
    }

    private void SerializeItems()
    {
        Debug.Log("Serializing items...");
        for (int i = 0; i < elements.Length; i++)
        {
            var el = elements[i];
            if (el == null)
            {
                Debug.LogWarning($"Element {i} is null in elements array.");
                continue;
            }

            int count = 0;

            if (el.itemId != 0) { count = KeyManager.Get_Item_Count(el.itemId); }
            else { count = 1; }

            if (el.button != null)
            {
                if (count <= 0)
                {
                    Debug.Log($"Slot {i} empty");
                    el.button.interactable = false;
                }
                else
                {
                    Debug.Log($"Slot {i} has {count}");
                    el.button.interactable = true;
                }
            }

            if (el.image != null) { el.image.color = (count <= 0) ? no_interactable : interactable; }
            if (el.countable && el.count != null) { el.count.text = count.ToString(); }
            else if (el.count != null) { el.count.text = ""; }
        }
    }

    internal void GetItemByTouch(int itemId)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            var el = elements[i];
            if (el != null && el.itemId == itemId)
            {
                _soundActions[i]?.Invoke();
                GetItem(i);
                return;
            }
        }
        
        Debug.LogWarning($"Item with ID {itemId} not found in inventory");
    }

    private void GetItem(int index)
    {
        if (index < 0 || index >= elements.Length)
        {
            Debug.LogWarning($"GetItem: index {index} out of range (elements.Length = {elements.Length}).");
            return;
        }

        Debug.Log($"Getting item... index in massive = {index}");
        var el = elements[index];
        if (el == null)
        {
            Debug.LogWarning($"GetItem: elements[{index}] is null.");
            return;
        }
        Debug.Log($"Item id = {el.itemId}");

        if (el.type == InstrumentType.None) { Debug.Log("This item no need to have any instrument"); }
        else if (el.type == InstrumentType.Axe)
        {
            Debug.Log("Checking axe power...");
            int power = KeyManager.GetInt_InstrumentPower_Axe();
            if (power >= el.needPower) { Debug.Log("You have need power to get item"); }
            else
            {
                SerializeItems();
                Debug.LogWarning("You haven't need power to get item");
                return;
            }
        }
        else if (el.type == InstrumentType.Pickaxe)
        {
            Debug.Log("Checking pickaxe power...");
            int power = KeyManager.GetInt_InstrumentPower_Pickaxe();
            if (power >= el.needPower) { Debug.Log("You have need power to get item"); }
            else
            {
                SerializeItems();
                Debug.LogWarning("You haven't need power to get item");
                return;
            }
        }

        if (el.countable) { KeyManager.Receive_Item(el.itemId, el.countGet); }
        else { KeyManager.Receive_Item_Once(el.itemId); }

        SerializeItems();
    }
}
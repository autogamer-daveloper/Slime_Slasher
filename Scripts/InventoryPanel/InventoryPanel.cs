using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryPanel : MonoBehaviour
{
    [Header("__ UI : main panel __")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button[] switchPanel;

    [Header("__ UI : other panel __")]
    [SerializeField] private RectTransform panelWeapon;
    [SerializeField] private RectTransform panelAccessories;
    [SerializeField] private RectTransform panelItems;

    [Header("__ Audio Source __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip openInv;
    [SerializeField] private AudioClip closeInv;
    [SerializeField] private AudioClip click;

    private bool _isHidden = true;
    private Vector2 showed = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);

    private bool isMoving = false;
    private bool isMutedFirst = true;
    private int selectedInventory = 0;

    private void OnEnable()
    {
        SelectInventory(2);

        foreach (Button btn in switchPanel) { btn.onClick.AddListener(Switch); }
    }

    private void OnDisable() { foreach (Button btn in switchPanel) { btn.onClick.RemoveListener(Switch); } }

    private void Switch()
    {
        if (_isHidden == true)
        {
            panel.DOAnchorPos(showed, 0.5f);
            src.PlayOneShot(openInv);
        }
        else
        {
            panel.DOAnchorPos(hidden, 0.5f);
            src.PlayOneShot(closeInv);
        }

        _isHidden = !_isHidden;
    }

    public void SelectInventory(int type)
    {
        if (selectedInventory == type) return;
        if (isMoving) return;

        isMoving = true;
        selectedInventory = type;
        Debug.Log("[InventoryPanel]: moving...");

        if (type == 0)
        {
            Debug.Log("[InventoryPanel]: moving to weapons...");
            panelWeapon.DOAnchorPos(showed, 0.5f);
            panelAccessories.DOAnchorPos(hidden, 0.5f);
            panelItems.DOAnchorPos(hidden, 0.5f);
        }

        if (type == 1)
        {
            Debug.Log("[InventoryPanel]: moving to accesories...");
            panelWeapon.DOAnchorPos(hidden, 0.5f);
            panelAccessories.DOAnchorPos(showed, 0.5f);
            panelItems.DOAnchorPos(hidden, 0.5f);
        }

        if (type == 2)
        {
            Debug.Log("[InventoryPanel]: moving to items...");
            panelWeapon.DOAnchorPos(hidden, 0.5f);
            panelAccessories.DOAnchorPos(hidden, 0.5f);
            panelItems.DOAnchorPos(showed, 0.5f);
        }

        Invoke("UnlockSwitchInventories", 0.5f);

        if (isMutedFirst) { isMutedFirst = false; return; }
        src.PlayOneShot(click);
    }

    private void UnlockSwitchInventories()
    {
        Debug.Log("[InventoryPanel]: unlocking...");
        isMoving = false;
    }
}

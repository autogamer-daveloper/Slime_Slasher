using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PortalActivating : MonoBehaviour
{
    [Header("__ Real portal __")]
    [SerializeField] private GameObject deactivatedPortal;
    [SerializeField] private GameObject activatedPortal;
    [Header("__ Paper portal __")]
    [SerializeField] private GameObject deactivatedPortalP;
    [SerializeField] private GameObject activatedPortalP;
    [Header("__ UI __")]
    [SerializeField] private Button activate;
    [SerializeField] private Button[] switchPanel;
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject help;
    [SerializeField] private Animation helpAnim;

    private string _isPortalActivated = "isPortalActivated";

    private int[] _itemsId = { 33, 34, 35 };
    private int[] _itemsCount = { 10, 10, 10 };

    private bool _isActive = false;

    private Vector2 shown = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);

    private void Start()
    {
        CheckStatus();
        foreach (Button btn in switchPanel) { btn.onClick.AddListener(Switch); }
        activate.onClick.AddListener(ActivatePortal);
    }

    private void OnDestroy()
    {
        foreach (Button btn in switchPanel) { btn.onClick.RemoveListener(Switch); }
        activate.onClick.RemoveListener(ActivatePortal);
    }

    private void BlockSwitch() { foreach (Button btn in switchPanel) { btn.interactable = false; } }
    private void UnlockSwitch() { foreach (Button btn in switchPanel) { btn.interactable = true; } }

    private void Switch()
    {
        CheckStatus();
        BlockSwitch();
        if (_isActive) { panel.DOAnchorPos(hidden, 0.5f); }
        else { panel.DOAnchorPos(shown, 0.5f); }
        Invoke(nameof(UnlockSwitch), 0.5f);
        _isActive = !_isActive;
    }

    private void CheckStatus()
    {
        int isAlreadyActivated = KeyManager.Get_Bool_Key(_isPortalActivated);
        if (isAlreadyActivated == 1) { deactivatedPortal.SetActive(false); activatedPortal.SetActive(true); return; }
        else { deactivatedPortal.SetActive(true); activatedPortal.SetActive(false); }

        bool canActivatePortal = CheckItems();
        if (canActivatePortal) { deactivatedPortalP.SetActive(false); activatedPortalP.SetActive(true); }
        else { deactivatedPortalP.SetActive(true); activatedPortalP.SetActive(false); }
    }

    private bool CheckItems()
    {
        for (int i = 0; i < _itemsId.Length; i++)
        {
            int index = i;
            int realCount = KeyManager.Get_Item_Count(_itemsId[index]);
            if (realCount < _itemsCount[index]) { return false; }
        }

        return true;
    }

    private void ActivatePortal()
    {
        int isAlreadyActivated = KeyManager.Get_Bool_Key(_isPortalActivated);
        if (isAlreadyActivated == 1) { help.SetActive(true); helpAnim.Play(); }
        else
        {
            for (int i = 0; i < _itemsId.Length; i++)
            {
                int index = i;
                KeyManager.Spend_Item(_itemsId[index], _itemsCount[index]);
            }
            KeyManager.Set_Bool_Key(_isPortalActivated, 1);
        }

        CheckStatus();
    }
}

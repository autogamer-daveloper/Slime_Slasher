using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmingSpot : MonoBehaviour
{
    [Header("__ Spot Settings __")]
    [SerializeField] private Button getButton;
    [SerializeField] private GameObject getButtonObj;
    [SerializeField] private int timer = 10;
    [SerializeField] private GameObject harvest;
    [Tooltip("If you will use particle, set this field.")]
    [SerializeField] private ParticleSystem particleEffect;
    [Header("__ Item settings __")]
    [SerializeField] private Inventory inv;
    [SerializeField] private int id = 20;

    private bool _isGrown = true;

    private void Start() { VisibilityOfHarvest(true); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (getButton != null && _isGrown == true)
            {
                getButtonObj.SetActive(true);
                getButton.onClick.AddListener(GetItem);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (getButton != null)
            {
                getButtonObj.SetActive(false);
                getButton.onClick.RemoveListener(GetItem);
            }
        }
    }

    private void VisibilityOfHarvest(bool answer)
    {
        harvest.SetActive(answer);
        _isGrown = answer;
    }

    private void GetItem()
    {
        bool canGet = CanGetItem();
        if (canGet == false) return;

        if (particleEffect != null) { particleEffect.Clear(); }
        if (particleEffect != null) { particleEffect.Play(); }

        getButtonObj.SetActive(false);
        getButton.onClick.RemoveListener(GetItem);

        VisibilityOfHarvest(false);
        Invoke("ShowHarvest", timer);
    }

    private void ShowHarvest() { VisibilityOfHarvest(true); }

    private bool CanGetItem()
    {
        if (inv == null)
        {
            Debug.LogError($"[FarmingSpot - {gameObject.name}]: can't continue instrument check, because of null param of 'inv'");
            return false;
        }

        if (inv.elements[id].type == InstrumentType.None) { return true; }
        else if (inv.elements[id].type == InstrumentType.Axe)
        {
            int axePower = KeyManager.GetInt_InstrumentPower_Axe();
            if (inv.elements[id].needPower <= axePower) { return true; }
            else { return false; }
        }
        else if (inv.elements[id].type == InstrumentType.Pickaxe)
        {
            int pickaxePower = KeyManager.GetInt_InstrumentPower_Pickaxe();
            if (inv.elements[id].needPower <= pickaxePower) { return true; }
            else { return false; }
        }
        else return true;
    }
}

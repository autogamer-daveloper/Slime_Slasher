using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    [Header("__ Main __")]
    [SerializeField] private GameObject forDelete;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject button;
    [SerializeField] private int id = 0;
    [Header("__ Experimental __")]
    [SerializeField] private bool getByTouch = true;

    // private string objName = "INVENTORY_FOUNDER";
    // private GameObject obj;
    // private InventoryFounder founder;

    private Button _button;

    private void Start()
    {
        if (button == null) { Debug.LogError("[GetObject.cs]:" + gameObject.name + " button hasn't selected!"); return; }
        _button = button.GetComponent<Button>();
        if (_button == null) { Debug.LogError("[GetObject.cs]:" + button.name + " object hasn't button component!"); }

        inventory = GameObject.FindFirstObjectByType<Inventory>();
        if (inventory == null) { Debug.LogError($"[GetObject {gameObject.name}]: can't find inventory script"); }

        // obj = GameObject.Find(objName);
        // if (obj != null)
        // {
        //     founder = obj.GetComponent<InventoryFounder>();
        //     if (founder != null) { button = founder.GetReceiveButtonById(id); }
        //     else { Debug.LogError($"[GetObject {gameObject.name}]: founder is null, something went wrong. Maybe 'obj' is empty"); }
        // }
        // else { Debug.LogError($"[GetObject {gameObject.name}]: not found 'obj' in scene, can't automatically get receive button"); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!getByTouch)
            {
                if (button != null)
                {
                    _button.onClick.AddListener(DestroyGameObject);
                    inventory.SerializeReceiveButtons();
                    button.SetActive(true);
                }
            }
            else
            {
                inventory.GetItemByTouch(id);
                DestroyGameObject();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!getByTouch)
            {
                if (button != null)
                {
                    inventory.SerializeReceiveButtons();
                    button.SetActive(false);
                }
            }
        }
    }

    private void DestroyGameObject()
    {
        if (button != null) { button.SetActive(false); }
        Destroy(forDelete);
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    [Header("__ Main __")]
    [Tooltip("This item 'game object' in world.")]
    [SerializeField] private GameObject forDelete;
    [Tooltip("Connect 'inventory' class")]
    [SerializeField] private Inventory inventory;
    [Tooltip("'Pick up' button for this item.")]
    [SerializeField] private GameObject button;
    [Tooltip("Item id in Inventory class for this scene.")]
    [SerializeField] private int id = 0;
    [Header("__ Experimental __")]
    [Tooltip("Get by touching item, or only by clicking button.")]
    [SerializeField] private bool getByTouch = true;
    [Tooltip("Will you call any method when you pick up item? (Work's only with 'getByTouch').")]
    [SerializeField] private bool useEvents = false;
    [Tooltip("Call some methods after picking up this item. (Work's only with 'getByTouch').")]
    [SerializeField] private UnityEvent actionAfterPickUp;

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
                if (useEvents) { actionAfterPickUp.Invoke(); }
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

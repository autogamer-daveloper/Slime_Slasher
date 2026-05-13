using UnityEngine;
using UnityEngine.UI;

public class InventoryFounder : MonoBehaviour
{
    [SerializeField] private GameObject[] buttonsById;

    internal GameObject GetReceiveButtonById(int id) { return buttonsById[id]; }
}

using UnityEngine;

[System.Serializable]
public class VisualAccessorySprites { public GameObject[] allSprites; }

public class PlayerVisualAccessory : MonoBehaviour
{
    [Header("__ Visual Accessories __")]
    [SerializeField] private VisualAccessorySprites[] types;

    private int _selectedVisualAccessory = 0;

    private void Start()
    {
        _selectedVisualAccessory = KeyManager.GetInt_VisualAccessoryID();
        Initialize();
    }

    private void Initialize()
    {
        foreach(VisualAccessorySprites type in types) { foreach (GameObject obj in type.allSprites) { obj.SetActive(false); } }
        if (_selectedVisualAccessory != 0) { foreach (GameObject obj in types[_selectedVisualAccessory - 1].allSprites) { obj.SetActive(true); } }
    }
}

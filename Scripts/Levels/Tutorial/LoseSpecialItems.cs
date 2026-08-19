using UnityEngine;

public class LoseSpecialItems : MonoBehaviour
{
    [SerializeField] private int[] idOfItems;

    private void Start()
    {
        foreach (int id in idOfItems)
        {
            int count = KeyManager.Get_Item_Count(id);
            KeyManager.Spend_Item(id, count);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SelectCraft : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Button[] select;

    private void Start()
    {
        for (int i = 0; i < select.Length; i++)
        {
            int id = i;
            select[id].onClick.AddListener(() => { SelectThis(id); });
        }
    }

    private void OnDestroy()
    {
        foreach (Button btn in select)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

    private void SelectThis(int id)
    {
        foreach (GameObject obj in panels)
        {
            obj.SetActive(false);
        }

        panels[id].SetActive(true);
    }
}

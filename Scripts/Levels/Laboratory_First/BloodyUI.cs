using UnityEngine;

public class BloodyUI : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private GameObject[] blood;

    private int _used = 0;

    public void GetBlood()
    {
        if (_used >= blood.Length) { return; }
        else { blood[_used].SetActive(true); _used++; }
    }
}

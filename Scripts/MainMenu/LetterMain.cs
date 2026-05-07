using UnityEngine;

public class LetterMain : MonoBehaviour
{
    [Header("__ Button __")]
    [SerializeField] private GameObject letterButton;
    [SerializeField] private string key = "LetterTriggered";

    private void Start()
    {
        int isTriggered = KeyManager.Get_Bool_Key(key);
        if (isTriggered == 1) { letterButton.SetActive(true); }
        else { letterButton.SetActive(false); }
    }
}

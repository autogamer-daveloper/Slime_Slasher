using UnityEngine;
using UnityEngine.UI;

public class LetterMain : MonoBehaviour
{
    [Header("__ Button __")]
    [SerializeField] private Button letterButton;
    [SerializeField] private string key = "LetterTriggered";

    private void Start()
    {
        int isTriggered = KeyManager.Get_Bool_Key(key);
        if (isTriggered == 1) { letterButton.interactable = true; }
        else { letterButton.interactable = false; }
    }
}

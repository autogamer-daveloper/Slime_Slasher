using UnityEngine;

public class LetterMain : MonoBehaviour
{
    [Header("__ Button __")]
    [SerializeField] private GameObject letterButton;

    private void Start()
    {
        int isTriggered = KeyManager.Get_Bool_Key("LetterTriggered");
        if (isTriggered == 1) { letterButton.SetActive(true); KeyManager.Set_Bool_Key("LetterTriggered", 0); }
        else { letterButton.SetActive(false); }
    }
}

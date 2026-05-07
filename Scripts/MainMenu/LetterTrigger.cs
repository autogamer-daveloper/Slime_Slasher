using UnityEngine;

public class LetterTrigger : MonoBehaviour
{
    [SerializeField] private string key = "LetterTriggered";

    private void Start() { KeyManager.Set_Bool_Key(key, 1); }
}
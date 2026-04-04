using UnityEngine;
using UnityEngine.UI;

public class PlayerTalkedWithZero : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private string key = "PlayerTalkedWithZero";
    [SerializeField] private Animation anim;
    [SerializeField] private Button talk;

    private int isTalked = 0;

    private void Start()
    {
        talk.onClick.AddListener(Talk);

        isTalked = KeyManager.Get_Bool_Key(key);
        if (isTalked == 1) { Talked(); }
    }

    private void OnDestroy()
    {
        talk.onClick.RemoveListener(Talk);
    }

    private void Talk()
    {
        KeyManager.Set_Bool_Key(key, 1);
    }

    private void Talked()
    {
        anim.Play();
    }
}

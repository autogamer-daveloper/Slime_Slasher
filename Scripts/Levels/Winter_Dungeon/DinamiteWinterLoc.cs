using UnityEngine;
using UnityEngine.UI;

public class DinamiteWinterLoc : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private Animation[] anims;
    [SerializeField] private GameObject[] opened;
    [SerializeField] private GameObject[] closed;
    [SerializeField] private string[] keys;
    [SerializeField] private Button[] activate;
    [SerializeField] private Button getTNT;
    [SerializeField] private Animation tip;
    [SerializeField] private GameObject tipObj;
    [SerializeField] private GameObject tnt;
    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip explosion;

    private int _clickedId;
    private bool _isDetonating = false;
    private bool _haveTNT = false;

    private void Start()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            int index = i;
            int isExplosed = KeyManager.Get_Bool_Key(keys[index]);
            if (isExplosed == 1) { _Activate(index); }

            activate[index].onClick.AddListener(() => { Activate(index); });
        }

        getTNT.onClick.AddListener(GetTNT);

        int haveTNT = KeyManager.Get_Bool_Key("haveTNT");
        if(haveTNT == 1) { _haveTNT = true; tnt.SetActive(false); }
    }

    private void OnDestroy()
    {
        foreach (Button btn in activate) { btn.onClick.RemoveAllListeners(); }
        getTNT.onClick.RemoveListener(GetTNT);
    }

    private void Activate(int id)
    {
        if (_isDetonating) return;

        if (_haveTNT)
        {
            anims[id].Play();
            KeyManager.Set_Bool_Key(keys[id], 1);
            _clickedId = id;
            _isDetonating = true;
            Invoke(nameof(_Activation), 5.5f);
            Invoke(nameof(PlaySound), 5f);
        }
        else
        {
            tipObj.SetActive(true);
            tip.Play();
        }
    }

    private void PlaySound(){ src.PlayOneShot(explosion); }

    private void _Activation()
    {
        _isDetonating = false;
        _Activate(_clickedId);
    }

    private void _Activate(int id)
    {
        opened[id].SetActive(true);
        closed[id].SetActive(false);
    }

    private void GetTNT()
    {
        _haveTNT = true;
        KeyManager.Set_Bool_Key("haveTNT", 1);
        tnt.SetActive(false);
    }
}

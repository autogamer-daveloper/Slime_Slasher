using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class NeedItems
{
    public int id;
    public int count;
}

public class SkeletonSpawner : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Button[] switchPanel;
    [Header("__ Simple boss __")]
    [SerializeField] private SkeletonLordBossFight simpleBoss;
    [SerializeField] private GameObject summonButton1;
    [SerializeField] private Button[] summoningButtons1;
    [SerializeField] private Button summonBoss1;
    [SerializeField] private NeedItems[] summonItems1;
    [SerializeField] private TMP_Text summoningText1;
    [Header("__ Hard boss __")]
    [SerializeField] private GameObject summonButton2;
    [SerializeField] private Button[] summoningButtons2;
    [SerializeField] private Button summonBoss2;
    [SerializeField] private NeedItems[] summonItems2;
    [SerializeField] private TMP_Text summoningText2;

    private Vector2 shown = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);

    private int _summon1Counted = 0;
    private int _summon2Counted = 0;
    private bool _isActive = false;
    private bool _isDefeated = false;
    private string _key;

    private void Start()
    {
        CheckDefeating();

        foreach (Button btn in switchPanel) { btn.onClick.AddListener(Switch); }
        foreach (Button btn in summoningButtons1) { btn.onClick.AddListener(CountSummoningNormalBoss); }
        foreach (Button btn in summoningButtons2) { btn.onClick.AddListener(CountSummoningHardBoss); }

        summonBoss1.onClick.AddListener(SummonBossNormal);
        summonBoss2.onClick.AddListener(SummonBossHard);
    }

    private void OnDestroy()
    {
        foreach (Button btn in switchPanel) { btn.onClick.RemoveListener(Switch); }
        foreach (Button btn in summoningButtons1) { btn.onClick.RemoveListener(CountSummoningNormalBoss); }
        foreach (Button btn in summoningButtons2) { btn.onClick.RemoveListener(CountSummoningHardBoss); }

        summonBoss1.onClick.RemoveListener(SummonBossNormal);
        summonBoss2.onClick.RemoveListener(SummonBossHard);
    }

    private void CheckDefeating()
    {
        _key = simpleBoss.GetBossKey();
        int isDefeatedBoss = KeyManager.Get_Bool_Key(_key);
        if (isDefeatedBoss == 1) { _isDefeated = true; }
        else _isDefeated = false;

        ShowCorrectPanel();
    }

    private void ShowCorrectPanel()
    {
        foreach (GameObject pan in panels) { pan.SetActive(false); }
        if (_isDefeated) { panels[1].SetActive(true); }
        else { panels[0].SetActive(true); }
    }

    private void Switch()
    {
        foreach (Button btn in switchPanel) { btn.interactable = false; }
        if (_isActive) { panel.DOAnchorPos(hidden, 0.5f).OnComplete(() => { foreach (Button btn in switchPanel) { btn.interactable = true; } }); }
        else { panel.DOAnchorPos(shown, 0.5f).OnComplete(() => { foreach (Button btn in switchPanel) { btn.interactable = true; } }); }

        foreach (Button btn in summoningButtons1) { btn.interactable = false; }
        foreach (Button btn in summoningButtons2) { btn.interactable = false; }

        _summon1Counted = 0;
        _summon2Counted = 0;
        summonButton1.SetActive(false);
        summonButton2.SetActive(false);
        summoningText1.text = "0";
        summoningText2.text = "0";

        _isActive = !_isActive;
        CheckDefeating();
        Invoke(nameof(ActivateRunes), 0.5f);
    }

    private void ActivateRunes()
    {
        foreach (Button btn in summoningButtons1) { btn.interactable = true; }
        foreach (Button btn in summoningButtons2) { btn.interactable = true; }
    }

    private void CountSummoningNormalBoss()
    {
        _summon1Counted++;
        summoningText1.text = _summon1Counted.ToString();
        if (_summon1Counted >= summoningButtons1.Length)
        {
            bool canSummon = CanSummonNormalBoss();
            if (canSummon) { summonButton1.SetActive(true); }
            else { } //Error
        }
    }

    private void CountSummoningHardBoss()
    {
        _summon2Counted++;
        summoningText2.text = _summon2Counted.ToString();
        if (_summon2Counted >= summoningButtons2.Length)
        {
            bool canSummon = CanSummonHardBoss();
            if (canSummon) { summonButton2.SetActive(true); }
            else { } //Error
        }
    }

    private bool CanSummonNormalBoss()
    {
        for (int i = 0; i < summonItems1.Length; i++)
        {
            int index = i;
            int realCount = KeyManager.Get_Item_Count(summonItems1[index].id);
            if (summonItems1[index].count > realCount) { return false; }
        }

        return true;
    }

    private bool CanSummonHardBoss()
    {
        for (int i = 0; i < summonItems2.Length; i++)
        {
            int index = i;
            int realCount = KeyManager.Get_Item_Count(summonItems2[index].id);
            if (summonItems2[index].count > realCount) { return false; }
        }

        return true;
    }

    private void SummonBossNormal()
    {
        foreach (NeedItems item in summonItems1)
        {
            KeyManager.Spend_Item(item.id, item.count);
        }
    }

    private void SummonBossHard()
    {
        foreach (NeedItems item in summonItems2)
        {
            KeyManager.Spend_Item(item.id, item.count);
        }
    }
}

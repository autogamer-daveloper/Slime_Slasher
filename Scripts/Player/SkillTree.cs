using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SkillType
{
    Health,
    Mana,
    Meele,
    Range,
    Mage
}

[System.Serializable]
public class SkillInTree
{
    public string upgradeId = "skill_0";
    public Button upgradeButton;
    public bool isNeedSkillsBefore = false;
    public bool isNeedOneUnlocked = false;
    public string[] skillsBefore;
    public Button[] unlockedButtons;
    public GameObject usingFlag;
    public SkillType type;
    public int upgrade = 0;
    public int upgradeLevel = 1;
}

public class SkillTree : MonoBehaviour
{
    [Header("__ Player Settings __")]
    [SerializeField] private PlayerStatus status;
    [SerializeField] private PlayerAttacking attack;
    [Header("__ Skills __")]
    [SerializeField] private SkillInTree[] skillSettings;
    [Header("__ UI __")]
    [SerializeField] private TMP_Text killedText;
    [SerializeField] private TMP_Text skillPointText;
    [SerializeField] private GameObject _helpLevelObj;
    [SerializeField] private Animation _helpLevel;
    [SerializeField] private GameObject _helpSkillsObj;
    [SerializeField] private Animation _helpSkills;
    [SerializeField] private Button[] canBeLocked;
    [SerializeField] private GameObject[] unusingFlags;
    [Header("__ Audio __")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip upgrade;
    [SerializeField] private AudioClip levelUp;

    private int _needKill = 15;
    private int _killed = 0;
    private int _skillPoint = 0;

    private void Start()
    {
        Initialize();
        InitializeButtons();
        InitializeSkills();
    }

    private void InitializeButtons()
    {
        foreach (SkillInTree setting in skillSettings) { setting.upgradeButton.onClick.RemoveAllListeners(); }

        for (int i = 0; i < skillSettings.Length; i++)
        {
            int index = i;
            skillSettings[index].upgradeButton.onClick.AddListener(() => { UpgradeSkill(index); });
        }
    }

    private void InitializeSkills()
    {
        foreach(Button btn in canBeLocked) { btn.interactable = false; }
        foreach(GameObject obj in unusingFlags) { obj.SetActive(false); }

        int HEALTH_LEVEL = KeyManager.Get_Bool_Key("skill_HEALTH_LEVEL");
        int MANA_LEVEL = KeyManager.Get_Bool_Key("skill_MANA_LEVEL");
        int MEELE_LEVEL = KeyManager.Get_Bool_Key("skill_MEELE_LEVEL");
        int RANGE_LEVEL = KeyManager.Get_Bool_Key("skill_RANGE_LEVEL");
        int MAGE_LEVEL = KeyManager.Get_Bool_Key("skill_MAGE_LEVEL");

        int searching_HEALTH_LEVEL = 0;
        int searching_MANA_LEVEL = 0;
        int searching_MEELE_LEVEL = 0;
        int searching_RANGE_LEVEL = 0;
        int searching_MAGE_LEVEL = 0;

        for (int i = 0; i < skillSettings.Length; i++)
        {
            int index = i;
            switch (skillSettings[index].type)
            {
                case SkillType.Health:
                    searching_HEALTH_LEVEL++;
                    if (searching_HEALTH_LEVEL == HEALTH_LEVEL)
                    {
                        KeyManager.Set_Bool_Key("skill_HEALTH", skillSettings[index].upgrade);
                        foreach (Button btn in skillSettings[index].unlockedButtons) { btn.interactable = true; }
                        skillSettings[index].usingFlag.SetActive(true);
                    }
                    break;
                case SkillType.Mana:
                    searching_MANA_LEVEL++;
                    if (searching_MANA_LEVEL == MANA_LEVEL)
                    {
                        KeyManager.Set_Bool_Key("skill_MANA", skillSettings[index].upgrade);
                        foreach (Button btn in skillSettings[index].unlockedButtons) { btn.interactable = true; }
                        skillSettings[index].usingFlag.SetActive(true);
                    }
                    break;
                case SkillType.Meele:
                    searching_MEELE_LEVEL++;
                    if (searching_MEELE_LEVEL == MEELE_LEVEL)
                    {
                        KeyManager.Set_Bool_Key("skill_MEELE", skillSettings[index].upgrade);
                        foreach (Button btn in skillSettings[index].unlockedButtons) { btn.interactable = true; }
                        skillSettings[index].usingFlag.SetActive(true);
                    }
                    break;
                case SkillType.Range:
                    searching_RANGE_LEVEL++;
                    if (searching_RANGE_LEVEL == RANGE_LEVEL)
                    {
                        KeyManager.Set_Bool_Key("skill_RANGE", skillSettings[index].upgrade);
                        foreach (Button btn in skillSettings[index].unlockedButtons) { btn.interactable = true; }
                        skillSettings[index].usingFlag.SetActive(true);
                    }
                    break;
                case SkillType.Mage:
                    searching_MAGE_LEVEL++;
                    if (searching_MAGE_LEVEL == MAGE_LEVEL)
                    {
                        KeyManager.Set_Bool_Key("skill_MAGE", skillSettings[index].upgrade);
                        foreach (Button btn in skillSettings[index].unlockedButtons) { btn.interactable = true; }
                        skillSettings[index].usingFlag.SetActive(true);
                    }
                    break;
            }
        }

        status.UpdateSkills();
        attack.UpdateSkills();
    }

    private void Initialize()
    {
        _killed = KeyManager.Get_Bool_Key("killed_enemies");
        _skillPoint = KeyManager.Get_Bool_Key("skill_points");
        UpdateUI();
    }

    private void UpgradeSkill(int id)
    {
        int skill = KeyManager.Get_Bool_Key(skillSettings[id].upgradeId);
        if (skill == 1) { return; }
        if (skillSettings[id].isNeedSkillsBefore == true)
        {
            int haveSkillsBefore = 0;
            for (int i = 0; i < skillSettings[id].skillsBefore.Length; i++)
            {
                int index = i;
                int checkSkill = KeyManager.Get_Bool_Key(skillSettings[id].skillsBefore[index]);
                if (!skillSettings[id].isNeedOneUnlocked) { if (checkSkill == 0) { _helpSkillsObj.SetActive(true); _helpSkills.Play(); return; } }
                else { if (checkSkill == 1) { haveSkillsBefore = 1; } }
            }

            if (skillSettings[id].isNeedOneUnlocked)
            {
                if (haveSkillsBefore == 0) { _helpSkillsObj.SetActive(true); _helpSkills.Play(); return; }
            }
        }
        if (_skillPoint <= 0) { _helpLevelObj.SetActive(true); _helpLevel.Play(); return; }

        _skillPoint -= 1;
        KeyManager.Set_Bool_Key("skill_points", _skillPoint);
        KeyManager.Set_Bool_Key(skillSettings[id].upgradeId, 1);
        src.PlayOneShot(upgrade);
        Initialize();
        switch (skillSettings[id].type)
        {
            case SkillType.Health:
                KeyManager.Set_Bool_Key("skill_HEALTH", skillSettings[id].upgrade);
                KeyManager.Set_Bool_Key("skill_HEALTH_LEVEL", skillSettings[id].upgradeLevel);
                break;
            case SkillType.Mana:
                KeyManager.Set_Bool_Key("skill_MANA", skillSettings[id].upgrade);
                KeyManager.Set_Bool_Key("skill_MANA_LEVEL", skillSettings[id].upgradeLevel);
                break;
            case SkillType.Meele:
                KeyManager.Set_Bool_Key("skill_MEELE", skillSettings[id].upgrade);
                KeyManager.Set_Bool_Key("skill_MEELE_LEVEL", skillSettings[id].upgradeLevel);
                break;
            case SkillType.Range:
                KeyManager.Set_Bool_Key("skill_RANGE", skillSettings[id].upgrade);
                KeyManager.Set_Bool_Key("skill_RANGE_LEVEL", skillSettings[id].upgradeLevel);
                break;
            case SkillType.Mage:
                KeyManager.Set_Bool_Key("skill_MAGE", skillSettings[id].upgrade);
                KeyManager.Set_Bool_Key("skill_MAGE_LEVEL", skillSettings[id].upgradeLevel);
                break;
        }
        InitializeSkills();
    }

    private void UpdateUI()
    {
        skillPointText.text = _skillPoint.ToString();
        killedText.text = _killed.ToString() + "/" + _needKill.ToString();
    }

    public void KilledEnemy()
    {
        if (_killed < _needKill - 1) {
            _killed += 1;
            KeyManager.Set_Bool_Key("killed_enemies", _killed);
        } else {
            _killed = 0;
            _skillPoint += 1;
            KeyManager.Set_Bool_Key("killed_enemies", _killed);
            KeyManager.Set_Bool_Key("skill_points", _skillPoint);
            src.PlayOneShot(levelUp);
        }

        UpdateUI();
    }
}

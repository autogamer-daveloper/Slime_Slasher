using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalSettings : MonoBehaviour
{
    [Header("__ UI: Settings __")]
    [Tooltip("Toggle: Is need to auto-use items: ( weapons / accessories / instruments ) after craft / bought?")]
    [SerializeField] private Toggle autoUse;
    [Tooltip("Toggle: show start warning to player?")]
    [SerializeField] private Toggle showWarning;
    [Tooltip("Audio volume change slider.")]
    [SerializeField] private Slider audioVolume;
    [Tooltip("Audio volume visual level.")]
    [SerializeField] private TMP_Text audioVolumeText;
    [Header("__ Global Audio Volume Setting __")]
    [Tooltip("Need for volume slider working.")]
    [SerializeField] private GlobalAudioVolumeSetting audioVolumeSetting;

    private const string autoUseKey = "IsNeedAutoUse";
    private const string showWarningKey = "IsShowWarning";
    private const string audioVolumeKey = "AudioVolume";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(autoUseKey)) { KeyManager.Set_Bool_Key(autoUseKey, 1); autoUse.isOn = true; }
        else
        {
            int tempNum = KeyManager.Get_Bool_Key(autoUseKey);
            switch (tempNum)
            {
                case 0:
                    autoUse.isOn = false;
                    break;
                case 1:
                    autoUse.isOn = true;
                    break;
                default:
                    autoUse.isOn = true;
                    break;
            }
        }

        if (!PlayerPrefs.HasKey(showWarningKey)) { KeyManager.Set_Bool_Key(showWarningKey, 1); showWarning.isOn = true; }
        else
        {
            int tempNum = KeyManager.Get_Bool_Key(showWarningKey);
            switch (tempNum)
            {
                case 0:
                    showWarning.isOn = false;
                    break;
                case 1:
                    showWarning.isOn = true;
                    break;
                default:
                    showWarning.isOn = true;
                    break;
            }
        }

        if (!PlayerPrefs.HasKey(audioVolumeKey)) { KeyManager.Set_Bool_Key(audioVolumeKey, 100); audioVolume.value = 100; }
        else
        {
            int tempNum = KeyManager.Get_Bool_Key(audioVolumeKey);
            audioVolume.value = tempNum;
        }

        int result = Mathf.FloorToInt(audioVolume.value + 0.5f);
        audioVolumeText.text = result.ToString();

        autoUse.onValueChanged.AddListener(onChangedAutoUse);
        showWarning.onValueChanged.AddListener(onChangedShowWarning);
        audioVolume.onValueChanged.AddListener(onChangedGlobalAudioVolume);
    }

    private void OnDestroy()
    {
        autoUse.onValueChanged.RemoveListener(onChangedAutoUse);
        showWarning.onValueChanged.RemoveListener(onChangedShowWarning);
        audioVolume.onValueChanged.RemoveListener(onChangedGlobalAudioVolume);
    }

    #region Settings change methods

    private void onChangedAutoUse(bool status)
    {
        int statusInt;
        if (status == true) { statusInt = 1; }
        else { statusInt = 0; }
        
        KeyManager.Set_Bool_Key(autoUseKey, statusInt);
    }

    private void onChangedShowWarning(bool status)
    {
        int statusInt;
        if (status == true) { statusInt = 1; }
        else { statusInt = 0; }
        
        KeyManager.Set_Bool_Key(showWarningKey, statusInt);
    }

    private void onChangedGlobalAudioVolume(float value)
    {
        int result = Mathf.FloorToInt(value + 0.5f);
        KeyManager.Set_Bool_Key(audioVolumeKey, result);
        audioVolumeText.text = result.ToString();
        audioVolumeSetting.Recalculate();
    }

    #endregion
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PostProcessingController : MonoBehaviour
{
    [Header("__ Is main menu controller? __")]
    [Tooltip("Is this component working in menu scene?")]
    [SerializeField] private bool isMainMenu = false;
    [Tooltip("Set 'Input type' component for correct work of this setting for first time.")]
    [SerializeField] private InputType inputType;

    [Space(5)]
    [Header("__ Component type: menu __")]
    [Tooltip("Toggle for post processing setting.")]
    [SerializeField] private Toggle toggle;
    [Tooltip("Audio source.")]
    [SerializeField] private AudioSource src;
    [Tooltip("Clip, which will play when changed settings (clicked to toggle).")]
    [SerializeField] private AudioClip clip;

    [Space(5)]
    [Header("__ Component type: other scene __")]
    [Tooltip("All volume components from this scene must be there.")]
    [SerializeField] private Volume[] volumes;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("PostProcessingEnabled"))
        {
            bool isMobile = inputType.IsMobileInput();
            if (isMobile)
            {
                //Disable volumes, when in other scenes, because main menu hasn't objects not in canvas, sorry for my english
                KeyManager.Set_Bool_Key("PostProcessingEnabled", 0);
                if (isMainMenu) { toggle.SetIsOnWithoutNotify(false); }
                else { foreach (Volume vol in volumes) { vol.gameObject.SetActive(false); } }
            }
            else
            {
                //Enable volumes, when in other scenes
                KeyManager.Set_Bool_Key("PostProcessingEnabled", 1);
                if (isMainMenu) { toggle.SetIsOnWithoutNotify(true); }
                else { foreach (Volume vol in volumes) { vol.gameObject.SetActive(true); } }
            }
        }
        else
        {
            int isEnabled = KeyManager.Get_Bool_Key("PostProcessingEnabled");
            if (isEnabled == 0)
            {
                if (isMainMenu) { toggle.SetIsOnWithoutNotify(false); }
                else { foreach (Volume vol in volumes) { vol.gameObject.SetActive(false); } }
            }
            else
            {
                if (isMainMenu) { toggle.SetIsOnWithoutNotify(true); }
                else { foreach (Volume vol in volumes) { vol.gameObject.SetActive(true); } }
            }
        }

        //Connect events for toggle with method
        toggle.onValueChanged.AddListener(OnSwitchedSetting);
    }

    //Disconnect events for toggle with method when object destroyed / game turned off
    private void OnDestroy() { toggle.onValueChanged.RemoveListener(OnSwitchedSetting); }

    private void OnSwitchedSetting(bool answer)
    {
        src.PlayOneShot(clip);
        if (answer) { KeyManager.Set_Bool_Key("PostProcessingEnabled", 1); }
        else { KeyManager.Set_Bool_Key("PostProcessingEnabled", 0); }
    }
}

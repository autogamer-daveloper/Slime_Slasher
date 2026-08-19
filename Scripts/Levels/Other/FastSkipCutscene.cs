using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class FastSkipCutscene : MonoBehaviour
{
    [Header("__ Skip button __")]
    [SerializeField] private Button skipButton;
    [Header("__ What to do? __")]
    [SerializeField] private bool isSceneLoader = true;
    [Space(10)]
    [SerializeField] private UnityEvent action;
    [Header("__ Scene loader __")]
    [SerializeField] private int sceneId = 0;
    [SerializeField] private GameObject loader;
    [Header("__ AudioSources for stop dialogue __")]
    [Tooltip("Optional. For stop dialogue sfx.")]
    [SerializeField] private AudioSource dialogueSFX;

    private float timer = 1f;

    private void Start()
    {
        skipButton.onClick.AddListener(SkipCutscene);
    }

    private void OnDestroy()
    {
        skipButton.onClick.RemoveListener(SkipCutscene);
    }

    private void SkipCutscene()
    {
        if(dialogueSFX != null) { dialogueSFX.Stop(); }
        StartCoroutine(LoadScene(sceneId, timer));
    }

    IEnumerator LoadScene(int id, float timer)
    {
        loader.SetActive(true);
        yield return new WaitForSeconds(timer);
        if (isSceneLoader)
        {
            LoadLevel.LoadLevelById(id);
        }
        else
        {
            action.Invoke();
        }
    }
}

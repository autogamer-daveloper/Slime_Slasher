using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class FastSkipCutscene : MonoBehaviour
{
    [Header("__ Skip button __")]
    [Tooltip("You can ignore this, Skip() method is public (Sometimes i use 'Events' without button, it's optional).")]
    [SerializeField] private Button skipButton; 
    [Header("__ What to do? __")]
    [SerializeField] private bool isSceneLoader = true;
    [Space(10)]
    [SerializeField] private UnityEvent action;
    [Header("__ Scene loader __")]
    [SerializeField] private int sceneId = 0;
    [SerializeField] private GameObject loader;
    [Header("__ Is delayed? __")]
    [Tooltip("(Optional). Set it if you want to delay action of fest skip button")]
    [SerializeField] private float delay = 0f;
    [Header("__ AudioSources for stop dialogue __")]
    [Tooltip("Optional. For stop dialogue sfx.")]
    [SerializeField] private AudioSource dialogueSFX;

    private float timer = 1f;

    private void Start()
    {
        if (skipButton != null) { skipButton.onClick.AddListener(SkipCutscene); }
    }

    private void OnDestroy()
    {
        if (skipButton != null) { skipButton.onClick.RemoveListener(SkipCutscene); }
    }

    public void Skip()
    {
        SkipCutscene();
    }

    private void SkipCutscene()
    {
        if (dialogueSFX != null) { dialogueSFX.Stop(); }
        StartCoroutine(LoadScene(sceneId, timer));
    }

    IEnumerator LoadScene(int id, float timer)
    {
        yield return new WaitForSeconds(delay);
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

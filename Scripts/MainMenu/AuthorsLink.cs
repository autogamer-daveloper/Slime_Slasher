using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AuthorsLink : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private Button yt;
    [SerializeField] private Button tg;
    [SerializeField] private Button website;
    [SerializeField] private Button openSource;
    [SerializeField] private Animation help;
    [SerializeField] private GameObject helpObj;
    //Links to links
    private const string _ytLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/YT.txt";
    private const string _tgLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/TG.txt";
    private const string _websiteLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/Website.txt";
    private const string _openSourceLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/OpenSource.txt";

    private string _ytLinkWeb;
    private string _tgLinkWeb;
    private string _websiteLinkWeb;
    private string _openSourceLinkWeb;

    private bool _ytWasLinked = false;
    private bool _tgWasLinked = false;
    private bool _websiteWasLinked = false;
    private bool _openSourceWasLinked = false;

    private void Start()
    {
        StartCoroutine(CheckYtLink());
        StartCoroutine(CheckTgLink());
        StartCoroutine(CheckWebsiteLink());
        StartCoroutine(CheckOpenSourceLink());

        yt.onClick.AddListener(OpenYT);
        tg.onClick.AddListener(OpenTG);
        website.onClick.AddListener(OpenWebsite);
        openSource.onClick.AddListener(OpenOpenSource);
    }

    private void OnDestroy()
    {
        yt.onClick.RemoveListener(OpenYT);
        tg.onClick.RemoveListener(OpenTG);
        website.onClick.RemoveListener(OpenWebsite);
        openSource.onClick.RemoveListener(OpenOpenSource);
    }

    #region LoadingLinks

    IEnumerator CheckYtLink()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_ytLink))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _ytLinkWeb = webRequest.downloadHandler.text.Trim();
                _ytWasLinked = true;
            }
            else
            {
                Debug.LogError("Error while loading 'YT' link: " + webRequest.error);
                _ytWasLinked = false;
            }
        }
    }

    IEnumerator CheckTgLink()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_tgLink))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _tgLinkWeb = webRequest.downloadHandler.text.Trim();
                _tgWasLinked = true;
            }
            else
            {
                Debug.LogError("Error while loading 'TG' link: " + webRequest.error);
                _tgWasLinked = false;
            }
        }
    }

    IEnumerator CheckWebsiteLink()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_websiteLink))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _websiteLinkWeb = webRequest.downloadHandler.text.Trim();
                _websiteWasLinked = true;
            }
            else
            {
                Debug.LogError("Error while loading 'Website' link: " + webRequest.error);
                _websiteWasLinked = false;
            }
        }
    }

    IEnumerator CheckOpenSourceLink()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_openSourceLink))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _openSourceLinkWeb = webRequest.downloadHandler.text.Trim();
                _openSourceWasLinked = true;
            }
            else
            {
                Debug.LogError("Error while loading 'Open source' link: " + webRequest.error);
                _openSourceWasLinked = false;
            }
        }
    }

    #endregion

    #region Buttons

    private void OpenYT()
    {
        if (_ytWasLinked) { Application.OpenURL(_ytLinkWeb); }
        else { Help(); StartCoroutine(CheckYtLink()); }
    }

    private void OpenTG()
    {
        if (_tgWasLinked) { Application.OpenURL(_tgLinkWeb); }
        else { Help(); StartCoroutine(CheckTgLink()); }
    }

    private void OpenWebsite()
    {
        if (_websiteWasLinked) { Application.OpenURL(_websiteLinkWeb); }
        else { Help(); StartCoroutine(CheckWebsiteLink()); }
    }

    private void OpenOpenSource()
    {
        if (_openSourceWasLinked) { Application.OpenURL(_openSourceLinkWeb); }
        else { Help(); StartCoroutine(CheckOpenSourceLink()); }
    }

    #endregion

    private void Help()
    {
        helpObj.SetActive(true);
        help.Play();
    }
}

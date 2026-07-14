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
    [SerializeField] private Button rateTheGame;
    [SerializeField] private Animation help;
    [SerializeField] private GameObject helpObj;
    [SerializeField] private GameObject windowReceivedRewardForRate;
    //Links to links
    private const string _ytLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/YT.txt";
    private const string _tgLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/TG.txt";
    private const string _websiteLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/Website.txt";
    private const string _openSourceLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/OpenSource.txt";
    private const string _gameLink = "https://raw.githubusercontent.com/autogamer-daveloper/slime-slasher-links/refs/heads/main/AppLink.txt";

    private string _ytLinkWeb;
    private string _tgLinkWeb;
    private string _websiteLinkWeb;
    private string _openSourceLinkWeb;
    private string _gameLinkWeb;

    private bool _ytWasLinked = false;
    private bool _tgWasLinked = false;
    private bool _websiteWasLinked = false;
    private bool _openSourceWasLinked = false;
    private bool _gameWasLinked = false;

    internal enum LinkType { Yt, Tg, Website, OpenSource, Game }

    private void Start()
    {
        StartCoroutine(CheckLink(_ytLink, LinkType.Yt));
        StartCoroutine(CheckLink(_tgLink, LinkType.Tg));
        StartCoroutine(CheckLink(_websiteLink, LinkType.Website));
        StartCoroutine(CheckLink(_openSourceLink, LinkType.OpenSource));
        StartCoroutine(CheckLink(_gameLink, LinkType.Game));

        yt.onClick.AddListener(OpenYT);
        tg.onClick.AddListener(OpenTG);
        website.onClick.AddListener(OpenWebsite);
        openSource.onClick.AddListener(OpenOpenSource);
        rateTheGame.onClick.AddListener(OpenGame);
    }

    private void OnDestroy()
    {
        yt.onClick.RemoveListener(OpenYT);
        tg.onClick.RemoveListener(OpenTG);
        website.onClick.RemoveListener(OpenWebsite);
        openSource.onClick.RemoveListener(OpenOpenSource);
        rateTheGame.onClick.RemoveListener(OpenGame);
    }

    #region LoadingLinks

    IEnumerator CheckLink(string linkSource, LinkType type)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(linkSource))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                switch (type)
                {
                    case LinkType.Yt:
                        _ytLinkWeb = webRequest.downloadHandler.text.Trim();
                        _ytWasLinked = true;
                        break;
                    case LinkType.Tg:
                        _tgLinkWeb = webRequest.downloadHandler.text.Trim();
                        _tgWasLinked = true;
                        break;
                    case LinkType.Website:
                        _websiteLinkWeb = webRequest.downloadHandler.text.Trim();
                        _websiteWasLinked = true;
                        break;
                    case LinkType.OpenSource:
                        _openSourceLinkWeb = webRequest.downloadHandler.text.Trim();
                        _openSourceWasLinked = true;
                        break;
                    case LinkType.Game:
                        _gameLinkWeb = webRequest.downloadHandler.text.Trim();
                        _gameWasLinked = true;
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case LinkType.Yt:
                        Debug.LogError("Error while loading 'YT' link: " + webRequest.error);
                        _ytWasLinked = false;
                        break;
                    case LinkType.Tg:
                        Debug.LogError("Error while loading 'TG' link: " + webRequest.error);
                        _tgWasLinked = false;
                        break;
                    case LinkType.Website:
                        Debug.LogError("Error while loading 'Website' link: " + webRequest.error);
                        _websiteWasLinked = false;
                        break;
                    case LinkType.OpenSource:
                        Debug.LogError("Error while loading 'Opensource' link: " + webRequest.error);
                        _openSourceWasLinked = false;
                        break;
                    case LinkType.Game:
                        Debug.LogError("Error while loading 'Game' link: " + webRequest.error);
                        _gameWasLinked = false;
                        break;
                }
            }
        }
    }

    #endregion

    #region Buttons

    private void OpenYT()
    {
        if (_ytWasLinked) { Application.OpenURL(_ytLinkWeb); }
        else { Help(); StartCoroutine(CheckLink(_ytLink, LinkType.Yt)); }
    }

    private void OpenTG()
    {
        if (_tgWasLinked) { Application.OpenURL(_tgLinkWeb); }
        else { Help(); StartCoroutine(CheckLink(_tgLink, LinkType.Tg)); }
    }

    private void OpenWebsite()
    {
        if (_websiteWasLinked) { Application.OpenURL(_websiteLinkWeb); }
        else { Help(); StartCoroutine(CheckLink(_websiteLink, LinkType.Website)); }
    }

    private void OpenOpenSource()
    {
        if (_openSourceWasLinked) { Application.OpenURL(_openSourceLinkWeb); }
        else { Help(); StartCoroutine(CheckLink(_openSourceLink, LinkType.OpenSource)); }
    }

    private void OpenGame()
    {
        if (_gameWasLinked)
        {
            windowReceivedRewardForRate.SetActive(true);
            KeyManager.Set_Bool_Key("visual_bought_5", 1);
            KeyManager.Set_Bool_Key("Rated", 1);
            Application.OpenURL(_gameLinkWeb);
        }
        else { Help(); StartCoroutine(CheckLink(_gameLink, LinkType.Game)); }
    }

    #endregion

    private void Help()
    {
        helpObj.SetActive(true);
        help.Play();
    }
}

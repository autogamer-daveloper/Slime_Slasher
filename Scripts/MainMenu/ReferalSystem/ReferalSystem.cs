using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ReferalSystem : MonoBehaviour
{
    [Header("__ Player ID Generator __")]
    [SerializeField] private PlayerIdGenerator playerIdGenerator;
    [Header("__ UI: Newbie __")]
    [SerializeField] private RectTransform newbiePanel;
    [SerializeField] private Button[] newbiePanButtons;
    [SerializeField] private TMP_InputField referalPlayerId;
    [SerializeField] private TMP_Text referalKeySend;
    [SerializeField] private Button copyReferalKey;
    [Header("__ UI: Referal invite __")]
    [SerializeField] private RectTransform referalPanel;
    [SerializeField] private Button[] referalPanButtons;
    [SerializeField] private TMP_InputField referalKey;
    [SerializeField] private TMP_Text playerIdSend;
    [SerializeField] private Button copyPlayerId;
    [Header("__ UI: Help __")]
    [SerializeField] private GameObject helpInviteId;
    [SerializeField] private Animation helpInviteIdAnim;
    [SerializeField] private GameObject helpReferalKey;
    [SerializeField] private Animation helpReferalKeyAnim;
    [Header("__ Successfully accepted referal __")]
    [Space(10)]
    [SerializeField] private UnityEvent acceptedReferal;

    private int friendPlayerId = 1000;
    private int generatedReferalKey = -1;

    private Vector2 shown = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);

    private const string isInvited = "IsInvitedBefore";

    private bool _shownN = false;
    private bool _shownR = false;

    private void Start()
    {
        playerIdSend.text = playerIdGenerator.GetPlayerId().ToString();

        ShowReferalKey(false);
        referalPlayerId.onEndEdit.AddListener(OnInvitedIdChanged);
        referalKey.onEndEdit.AddListener(OnReferalKeyChanged);
        copyPlayerId.onClick.AddListener(CopyYourPlayerId);
        copyReferalKey.onClick.AddListener(CopyFriendReferalKey);

        foreach(Button btn in newbiePanButtons) { btn.onClick.AddListener(ChangeStateNewbiePanel); }
        foreach(Button btn in referalPanButtons) { btn.onClick.AddListener(ChangeStateReferalPanel); }

        if (PlayerPrefs.HasKey(isInvited))
        {
            newbiePanel.DOAnchorPos(hidden, 0.25f);
            _shownN = false;
        }
        else
        {
            newbiePanel.DOAnchorPos(shown, 0.25f);
            _shownN = true;
        }
    }

    private void OnDestroy()
    {
        referalPlayerId.onEndEdit.RemoveListener(OnInvitedIdChanged);
        referalKey.onEndEdit.RemoveListener(OnReferalKeyChanged);
        copyPlayerId.onClick.RemoveListener(CopyYourPlayerId);
        copyReferalKey.onClick.RemoveListener(CopyFriendReferalKey);

        foreach(Button btn in newbiePanButtons) { btn.onClick.RemoveListener(ChangeStateNewbiePanel); }
        foreach(Button btn in referalPanButtons) { btn.onClick.RemoveListener(ChangeStateReferalPanel); }
    }

    #region UI Panels

    private void ChangeStateNewbiePanel()
    {
        if (_shownN) { newbiePanel.DOAnchorPos(hidden, 0.25f); }
        else { newbiePanel.DOAnchorPos(shown, 0.25f); }

        _shownN = !_shownN;

        KeyManager.Set_Bool_Key(isInvited, 1);
    }

    private void ChangeStateReferalPanel()
    {
        if (_shownR) { referalPanel.DOAnchorPos(hidden, 0.25f); }
        else { referalPanel.DOAnchorPos(shown, 0.25f); }

        _shownR = !_shownR;
    }

    #endregion

    #region UI InputFields

    private void OnInvitedIdChanged(string value)
    {
        if (int.TryParse(value, out int id))
        {
            if (id < 1000 || id > 4999)
            {
                referalPlayerId.text = "";
                helpInviteId.SetActive(true);
                helpInviteIdAnim.Play();
                ShowReferalKey(false);
                Debug.LogWarning("Invalid invited friend's id, Please retry");
                return;
            }
            else
            {
                friendPlayerId = id;
                GenerateReferalKey();
                ShowReferalKey(true);
            }
        }
    }

    private void OnReferalKeyChanged(string value)
    {
        if (int.TryParse(value, out int id))
        {
            if (id < 10000000 || id > 99999999)
            {
                referalKey.text = "";
                Debug.LogWarning("Invalid referal key, Please retry");
                return;
            }
            else
            {
                bool isDecoded = DecodeReferalKey(id);
                if (isDecoded)
                {
                    acceptedReferal.Invoke();
                }
                else
                {
                    helpReferalKey.SetActive(true);
                    helpReferalKeyAnim.Play();
                }
            }
        }
    }

    #endregion

    #region UI Buttons

    private void CopyYourPlayerId()
    {
        int yourPlayerId = playerIdGenerator.GetPlayerId();
        GUIUtility.systemCopyBuffer = yourPlayerId.ToString();
    }

    private void CopyFriendReferalKey()
    {
        if (generatedReferalKey != -1)
        {
            GUIUtility.systemCopyBuffer = generatedReferalKey.ToString();
        }
    }

    #endregion

    #region UI Other

    private void ShowReferalKey(bool isFilled)
    {
        if (isFilled)
        {
            referalKeySend.text = generatedReferalKey.ToString();
            copyReferalKey.interactable = true;
        }
        else
        {
            referalKeySend.text = "";
            copyReferalKey.interactable = false;
        }
    }

    #endregion

    #region Referal key decoder

    private bool DecodeReferalKey(int decodingKey)
    {
        int yourPlayerId = playerIdGenerator.GetPlayerId();

        int keyPart1 = decodingKey / 10000;
        int keyPart2 = decodingKey % 10000;

        int friendId = keyPart1 - yourPlayerId;
        if (friendId < 1000 || friendId > 4999)
        {
            Debug.LogWarning("Friend's playerId is invalid");
            return false;
        }

        bool isSecondPartValid = DecodeSecondPart(yourPlayerId, keyPart2);
        if (!isSecondPartValid) { Debug.LogWarning("Referal key is invalid"); return false; }

        Debug.Log("Referal key is valid, you will rewarded soon");
        return true;
    }

    private bool DecodeSecondPart(int playerId, int keyPart2)
    {
        int[] playerDigits = new int[4];
        int[] keyDigits = new int[4];

        bool[] playerEven = new bool[4];
        bool[] keyEven = new bool[4];

        playerDigits[0] = playerId / 1000;
        playerDigits[1] = playerId / 100 % 10;
        playerDigits[2] = playerId / 10 % 10;
        playerDigits[3] = playerId % 10;

        keyDigits[0] = keyPart2 / 1000;
        keyDigits[1] = keyPart2 / 100 % 10;
        keyDigits[2] = keyPart2 / 10 % 10;
        keyDigits[3] = keyPart2 % 10;

        playerEven[0] = playerDigits[0] % 2 == 0;
        playerEven[1] = playerDigits[1] % 2 == 0;
        playerEven[2] = playerDigits[2] % 2 == 0;
        playerEven[3] = playerDigits[3] % 2 == 0;

        keyEven[0] = keyDigits[0] % 2 == 0;
        keyEven[1] = keyDigits[1] % 2 == 0;
        keyEven[2] = keyDigits[2] % 2 == 0;
        keyEven[3] = keyDigits[3] % 2 == 0;

        for (int i = 0; i < playerDigits.Length; i++)
        {
            int id = i;
            if (playerEven[id] != keyEven[id])
            {
                return false;
            }
            else if (playerDigits[id] == keyDigits[id])
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Referal key generation

    private void GenerateReferalKey()
    {
        int yourPlayerId = playerIdGenerator.GetPlayerId();

        int keyPart1 = yourPlayerId + friendPlayerId;
        int keyPart2 = GenerateSecondPartKey(yourPlayerId);

        generatedReferalKey = (keyPart1 * 10000) + keyPart2;
    }

    #endregion

    #region Work with numbers

    private int GenerateSecondPartKey(int playerId)
    {
        bool[] evenDigits = new bool[4];
        int[] exceptNumbers = new int[4];
        int[] newGeneratedNumbers = new int[4];

        exceptNumbers[0] = playerId / 1000;
        exceptNumbers[1] = playerId / 100 % 10;
        exceptNumbers[2] = playerId / 10 % 10;
        exceptNumbers[3] = playerId % 10;

        evenDigits[0] = exceptNumbers[0] % 2 == 0;
        evenDigits[1] = exceptNumbers[1] % 2 == 0;
        evenDigits[2] = exceptNumbers[2] % 2 == 0;
        evenDigits[3] = exceptNumbers[3] % 2 == 0;

        for (int i = 0; i < exceptNumbers.Length; i++)
        {
            int id = i;
            if (evenDigits[id])
            {
                newGeneratedNumbers[id] = GenerateNumberExcept_Even(exceptNumbers[id]);
            }
            else
            {
                newGeneratedNumbers[id] = GenerateNumberExcept_Odd(exceptNumbers[id]);
            }
        }

        int secondPart = (newGeneratedNumbers[0] * 1000) +
                        (newGeneratedNumbers[1] * 100) +
                        (newGeneratedNumbers[2] * 10) +
                        newGeneratedNumbers[3];

        return secondPart;
    }

    private int GenerateNumberExcept_Odd(int exceptNum)
    {
        int rand;

        do { rand = Random.Range(0, 10); }
        while (rand % 2 == 0 || rand == exceptNum);

        return rand;
    }

    private int GenerateNumberExcept_Even(int exceptNum)
    {
        int rand;

        do { rand = Random.Range(0, 10); }
        while (rand % 2 != 0 || rand == exceptNum);

        return rand;
    }

    #endregion
}

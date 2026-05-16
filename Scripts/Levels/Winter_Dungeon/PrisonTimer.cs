using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class SetTimeForClocks
{
    [Range(0, 59)]
    public int minutes;
    [Range(0, 59)]
    public int seconds;
}

public class PrisonTimer : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private Transform prison;
    [SerializeField] private Transform work;
    [Header("__ UI __")]
    [SerializeField] private Animation loadPanel;
    [SerializeField] private TMP_Text timer_text;
    [SerializeField] private GameObject timer_obj;
    [Header("__ UI helpers __")]
    [SerializeField] private Animation inPrisonHelp;
    [SerializeField] private Animation atWorkHelp;
    [SerializeField] private GameObject inPrisonHelpObj;
    [SerializeField] private GameObject atWorkHelpObj;
    [Header("__ Time: count for being in prison __")]
    [SerializeField] private SetTimeForClocks inPrison;
    [Header("__ Time: count for being at work __")]
    [SerializeField] private SetTimeForClocks atWork;
    [Header("__ Autokill __")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject autoKiller;

    private bool _isCompletedTask = true;
    private bool _inPrison = true;
    private int _minutes = 0;
    private int _seconds = 0;

    private Vector2 shown = new Vector2(0, 0);
    private Vector2 hidden = new Vector2(0, -2000);

    private void Start()
    {
        SetTimer(0);
        inPrisonHelpObj.SetActive(true);
        inPrisonHelp.Play();
    }

    private void Count()
    {
        if (_seconds <= 0)
        {
            if (_minutes <= 0)
            {
                CheckStatus();
                CancelInvoke(nameof(Count));
            }
            else
            {
                _minutes -= 1;
                _seconds = 59;
            }
        }
        else
        {
            _seconds -= 1;
        }

        if (_seconds <= 9) { timer_text.text = _minutes.ToString() + ":0" + _seconds.ToString(); }
        else timer_text.text = _minutes.ToString() + ":" + _seconds.ToString();
    }

    private void SetTimer(int id)
    {
        switch (id)
        {
            case 0:
                _minutes = inPrison.minutes;
                _seconds = inPrison.seconds;
                break;
            case 1:
                _minutes = atWork.minutes;
                _seconds = atWork.seconds;
                break;
            default:
                _minutes = inPrison.minutes;
                _seconds = inPrison.seconds;
                break;
        }

        CancelInvoke(nameof(Count));
        InvokeRepeating(nameof(Count), 1, 1);

        if (_seconds <= 9) { timer_text.text = _minutes.ToString() + ":0" + _seconds.ToString(); }
        else timer_text.text = _minutes.ToString() + ":" + _seconds.ToString();
    }

    private void CheckStatus()
    {
        loadPanel.Play();
        if (_inPrison) { Invoke(nameof(GoToWork), 0.5f); }
        else { Invoke(nameof(GoToPrison), 0.5f); }
    }

    private void GoToWork()
    {
        _inPrison = false;
        player.position = work.position;
        SetTimer(1);
        atWorkHelpObj.SetActive(true);
        atWorkHelp.Play();
    }

    private void GoToPrison()
    {
        if (!_isCompletedTask) { Instantiate(autoKiller, player.position, player.rotation); }
        _isCompletedTask = false;

        player.position = prison.position;
        SetTimer(0);
        inPrisonHelpObj.SetActive(true);
        inPrisonHelp.Play();
    }

    public void CompleteTask() { _isCompletedTask = true; }

    public void Dead() { Invoke(nameof(_Dead), 1f); }

    private void _Dead()
    {
        _inPrison = true;
        _isCompletedTask = false;
        // SetTimer(0);
        // inPrisonHelpObj.SetActive(true);
        // inPrisonHelp.Play();
        // PlayTimer();
        PauseTimer();
    }

    public void PauseTimer()
    {
        CancelInvoke(nameof(Count));
        timer_obj.SetActive(false);
    }

    public void PlayTimer()
    {
        InvokeRepeating(nameof(Count), 1, 1);
        timer_obj.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ActivateTrapsNightmare : MonoBehaviour
{
    [Header("__ To Disable __")]
    [SerializeField] private GameObject[] toDis;
    [Header("__ To Enable __")]
    [SerializeField] private GameObject[] toEn;
    [SerializeField] private GameObject[] toEnNow;
    [Header("__ Activator __")]
    [SerializeField] private Button activate;
    [Header("__ Timer __")]
    [SerializeField] private TMP_Text timer_text;
    [SerializeField] private UnityEvent Alive;
    [SerializeField] private UnityEvent Dead;

    private int _minutes = 0;
    private int _seconds = 45;

    private void Start() { activate.onClick.AddListener(ActivateTrap); }

    private void OnDestroy() { activate.onClick.RemoveListener(ActivateTrap); }

    private void ActivateTrap()
    {
        foreach (GameObject en in toEnNow) { en.SetActive(true); }
        Invoke(nameof(ActivateWithDelay), 0.5f);
    }

    private void ActivateWithDelay()
    {
        foreach (GameObject dis in toDis) { dis.SetActive(false); }
        foreach (GameObject en in toEn) { en.SetActive(true); }

        InvokeRepeating(nameof(Count), 1, 1);
    }

    private void Count()
    {
        if (_seconds <= 0)
        {
            if (_minutes <= 0)
            {
                Alive.Invoke();
                CancelInvoke(nameof(Count));
            }
            else
            {
                _minutes -= 1;
                _seconds = 59;
            }
        }
        else { _seconds -= 1; }

        if(_seconds <= 9) { timer_text.text = _minutes.ToString() + ":0" + _seconds.ToString(); }
        else timer_text.text = _minutes.ToString() + ":" + _seconds.ToString();
    }

    internal void DeadSlime()
    {
        Dead.Invoke();
        CancelInvoke(nameof(Count));
    }
}

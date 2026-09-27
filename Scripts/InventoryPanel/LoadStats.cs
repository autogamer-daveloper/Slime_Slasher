using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadStats : MonoBehaviour
{
    [Tooltip("Select text source class.")]
    [SerializeField] private TMP_Text textSource;
    [Tooltip("Enter custom value.")]
    [SerializeField] private int yourValue;
    [Tooltip("Use only if you need prefix before value.")]
    [SerializeField] private bool isUsingPrefix = false;
    [Tooltip("Your custom prefix before value.")]
    [SerializeField] private string prefix;

    private void Start()
    {
        if (isUsingPrefix) { textSource.text = textSource.text + " " + prefix + yourValue.ToString(); }
        else { textSource.text = textSource.text + " " + yourValue.ToString(); }
    }
}

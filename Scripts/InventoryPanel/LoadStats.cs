using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadStats : MonoBehaviour
{
    [Tooltip("Select text source class.")]
    [SerializeField] private TMP_Text textSource;
    [Tooltip("Enter custom value.")]
    [SerializeField] private int yourValue;

    private void Start() { textSource.text = textSource.text + " " + yourValue.ToString(); }
}

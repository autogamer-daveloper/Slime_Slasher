using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    [SerializeField] private Button btn;

    private void OnEnable()
    {
        btn.interactable = false;
        Invoke(nameof(Activate), 1f);
    }

    private void Activate()
    {
        btn.interactable = true;
    }
}

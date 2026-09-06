using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnimation : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("Scale")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 0.9f;

    private Vector3 defaultScale;
    private bool isHovered;
    private bool isPressed;

    private void Awake() { defaultScale = transform.localScale; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (!isPressed) { transform.localScale = defaultScale * hoverScale; }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        if (!isPressed) { transform.localScale = defaultScale; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        transform.localScale = defaultScale * clickScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        transform.localScale = isHovered ? defaultScale * hoverScale : defaultScale;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class LookArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool IsDragging { get; private set; }
    public Vector2 Delta { get; private set; }

    private Vector2 lastPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;
        lastPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Delta = eventData.position - lastPosition;
        lastPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsDragging = false;
        Delta = Vector2.zero;
    }

    private void LateUpdate()
    {
        Delta = Vector2.zero;
    }
}
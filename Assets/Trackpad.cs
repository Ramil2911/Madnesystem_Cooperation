using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trackpad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 lastDelta;
    public float sensitivity = 0.01f;

    private int touchId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        touchId = eventData.pointerId;
        lastDelta = default;
    }

    private void Update()
    {
        if(touchId == -1) return;
        lastDelta = Input.GetTouch(touchId).deltaPosition * sensitivity;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == touchId) touchId = -1;
        lastDelta = default;
    }
}

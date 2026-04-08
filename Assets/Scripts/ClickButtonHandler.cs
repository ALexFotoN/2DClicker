using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnHoldStart;
    public event Action OnHoldEnd;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnHoldStart?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnHoldEnd?.Invoke();
    }
}
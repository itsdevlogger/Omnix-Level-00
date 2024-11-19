using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AndroidInputs.InputReaders
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class ButtonInput : MaskableGraphic, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnPressed;
        public UnityEvent OnReleased;
        public bool IsPressed { get; private set; }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsPressed)
            {
                IsPressed = false;
                OnReleased?.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsPressed)
            {
                IsPressed = false;
                OnReleased?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPressed = true;
            OnPressed?.Invoke();
        }
    }
}
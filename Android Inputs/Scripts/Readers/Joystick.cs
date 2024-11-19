using AndroidInputs.Modifiers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace AndroidInputs.InputReaders
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform _handle;
        [SerializeField] private float _joystickRadius = 300f;
        [SerializeReference] private IInputModifier[] _modifier;
        
        private RectTransform _baseRect;
        private Vector2 _value = Vector2.zero;
        
        public event Action<Vector2> OnRecieved;
        public Vector2 ValueXY => _modifier.Modify(_value);
        public float ValueX => _modifier.Modify(_value.x);
        public float ValueY => _modifier.Modify(_value.y);

        private void Start()
        {
            _baseRect = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pointerPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, eventData.position, eventData.pressEventCamera, out pointerPos);

            if (pointerPos.magnitude > _joystickRadius)
                pointerPos = pointerPos.normalized * _joystickRadius;

            _handle.anchoredPosition = pointerPos;
            _value = pointerPos / _joystickRadius;
            OnRecieved?.Invoke(_modifier.Modify(_value));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _handle.anchoredPosition = Vector2.zero;
            _value = Vector2.zero;
        }
    }
}
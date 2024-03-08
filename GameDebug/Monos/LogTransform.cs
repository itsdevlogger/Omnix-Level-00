using System;
using UnityEngine;

namespace DebugToScreen
{
    public class LogTransform : MonoBehaviour
    {
        private enum Property
        {
            localPosition,
            localRotation,
            localScale,
            position,
            rotation,
            lossyScale
        }

        [SerializeField] private Property property;
        [SerializeField] private string prefix;
        [SerializeField] private string suffix;
        private Message _message;
        private Func<string> _getterFunc;

        void Start()
        {
            switch (property)
            {
                case (Property.localPosition):
                {
                    _getterFunc = () => transform.localPosition.ToString();
                    break;
                }
                case (Property.localRotation):
                {
                    _getterFunc = () => transform.localRotation.ToString();
                    break;
                }
                case (Property.localScale):
                {
                    _getterFunc = () => transform.localScale.ToString();
                    break;
                }
                case (Property.position):
                {
                    _getterFunc = () => transform.position.ToString();
                    break;
                }
                case (Property.rotation):
                {
                    _getterFunc = () => transform.rotation.ToString();
                    break;
                }
                case (Property.lossyScale):
                {
                    _getterFunc = () => transform.lossyScale.ToString();
                    break;
                }
            }
            _message = GameDebug.Log($"{prefix}{_getterFunc()}{suffix}");
        }

        void Update()
        {
            _message.Text = $"{prefix}{_getterFunc()}{suffix}";
        }
    }
}
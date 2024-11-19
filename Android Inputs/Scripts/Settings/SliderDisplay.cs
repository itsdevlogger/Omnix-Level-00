using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AndroidInputs.InputSettings
{
    [RequireComponent(typeof(Slider))]
    public class SliderDisplay : MonoBehaviour
    {
        // @formatter:off
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private string _format = "0.00";
        [Header("Interpolate")] 
        [SerializeField] private bool _interpolateValue = false;
        [SerializeField] private float _minDisplayValue = 0f;
        [SerializeField] private float _maxDisplayValue = 1f;
        
        private Slider _slider;
        // @formatter:on

        private void OnEnable()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(SetDisplayText);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(SetDisplayText);
        }

        private float GetDisplayValue()
        {
            if (_interpolateValue)
            {
                if (Mathf.Approximately(_slider.minValue, _slider.maxValue)) return _slider.value;
                return (_slider.value - _slider.minValue) / (_slider.maxValue - _slider.minValue) * (_maxDisplayValue - _minDisplayValue) + _minDisplayValue;
            }
            return _slider.value;
        }

        private void SetDisplayText(float _)
        {
            if (_displayText != null)
            {
                var value = GetDisplayValue();
                _displayText.SetText(value.ToString(_format));
            }
        }
    }
}
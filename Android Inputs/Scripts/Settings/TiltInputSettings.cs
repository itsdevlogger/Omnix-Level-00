using AndroidInputs.InputReaders;
using UnityEngine;
using UnityEngine.UI;

namespace AndroidInputs.InputSettings
{
    public class TiltInputSettings : MonoBehaviour
    {
        // @formatter:off
        [Header("Tilt Input Reference")] 
        [Tooltip("[CanBeNull] Target Input")]
        public TiltInput target;

        [Tooltip("PlayerPref Key of the target input, this key will be used to update the settings saved in player prefs (in case target is null)")]
        public string targetPlayerPrefKey;

        [Header("Slider Settings")] public float minSensitivity = 0.5f;
        public float maxSensitivity = 5f;
        public float minTolerance = 10f;
        public float maxTolerance = 90f;

        [Header("Sliders")] public Slider tiltSensitivitySlider;
        public Slider maxTiltAngleSlider;
        // @formatter:on


        private void Start()
        {
            // Set slider min and max values regardless of tiltInput
            tiltSensitivitySlider.minValue = minSensitivity;
            tiltSensitivitySlider.maxValue = maxSensitivity;
            maxTiltAngleSlider.minValue = minTolerance;
            maxTiltAngleSlider.maxValue = maxTolerance;

            if (target != null)
            {
                // If tiltInput is available, set slider values from it
                tiltSensitivitySlider.value = target.sensitivity;
                maxTiltAngleSlider.value = target.tolerance;
                targetPlayerPrefKey = target.playerPrefKey;
            }
            else
            {
                // If tiltInput is null, load values from PlayerPrefs
                float defaultTiltSensitivity = PlayerPrefs.GetFloat($"{targetPlayerPrefKey}+0", 2f);
                float defaultMaxTiltAngle = PlayerPrefs.GetFloat($"{targetPlayerPrefKey}+1", 45f);

                tiltSensitivitySlider.value = defaultTiltSensitivity;
                maxTiltAngleSlider.value = defaultMaxTiltAngle;
            }

            // Add listeners for slider value changes
            tiltSensitivitySlider.onValueChanged.AddListener(OnTiltSensitivityChanged);
            maxTiltAngleSlider.onValueChanged.AddListener(OnMaxTiltAngleChanged);
        }

        private void OnValidate()
        {
            if (tiltSensitivitySlider != null)
            {
                tiltSensitivitySlider.minValue = minSensitivity;
                tiltSensitivitySlider.maxValue = maxSensitivity;
            }
            else
            {
                Debug.LogError("[TiltInputSettings] tiltSensitivitySlider is null", gameObject);
            }

            if (maxTiltAngleSlider != null)
            {
                maxTiltAngleSlider.minValue = minTolerance;
                maxTiltAngleSlider.maxValue = maxTolerance;
            }
            else
            {
                Debug.LogError("[TiltInputSettings] maxTiltAngleSlider is null", gameObject);
            }
        }

        private void Reset()
        {
            tiltSensitivitySlider = new GameObject("Tilt Sensitivity").AddComponent<Slider>();
            maxTiltAngleSlider = new GameObject("Max Tilt Angle").AddComponent<Slider>();

            tiltSensitivitySlider.transform.SetParent(transform);
            maxTiltAngleSlider.transform.SetParent(transform);
        }

        private void OnTiltSensitivityChanged(float value)
        {
            if (target != null)
            {
                target.sensitivity = value;
            }

            PlayerPrefs.SetFloat($"{targetPlayerPrefKey}+0", value);
        }

        private void OnMaxTiltAngleChanged(float value)
        {
            if (target != null)
            {
                target.tolerance = value;
            }

            PlayerPrefs.SetFloat($"{targetPlayerPrefKey}+1", value);
        }
    }
}
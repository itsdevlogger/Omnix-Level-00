using AndroidInputs.Modifiers;
using UnityEngine;

namespace AndroidInputs.InputReaders
{
    public class TiltInput : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("How fast should the system reach max value")] public float sensitivity = 2f;
        [Tooltip("Changes in value less than tolerance will be ignored.")] public float tolerance = 0.05f;
        
        [Header("Save/Load")]
        [Tooltip("Should the \"Settings\" be stored to player prefs.")] public bool autosave = true;
        [Tooltip("Key to be used for storing values of this input to PlayerPref.")] public string playerPrefKey;
        
        [SerializeReference] private IInputModifier[] _modifier;

        public Vector2 ValueXY
        {
            get
            {
                var tiltX = Mathf.Round(Input.acceleration.x * sensitivity / tolerance) * tolerance;
                var tiltY = Mathf.Round(Input.acceleration.y * sensitivity / tolerance) * tolerance;
                return _modifier.Modify(new Vector2(tiltX, tiltY));
            }
        }
        
        public float ValueX
        {
            get
            {
                var tiltX = Mathf.Round(Input.acceleration.x * sensitivity / tolerance) * tolerance;
                return _modifier.Modify(tiltX);
            }
        }
        
        public float ValueY
        {
            get
            {
                var tiltY = Mathf.Round(Input.acceleration.y * sensitivity / tolerance) * tolerance;
                return _modifier.Modify(tiltY);
            }
        }
        
        private void OnEnable()
        {
            if (autosave)
            {
                LoadFromPlayerPrefs();
            }
        }

        public void SaveToPlayerPrefs()
        {
            PlayerPrefs.SetFloat($"{playerPrefKey}/0", sensitivity);
            PlayerPrefs.SetFloat($"{playerPrefKey}/1", tolerance);
        }

        public void LoadFromPlayerPrefs()
        {
            sensitivity = PlayerPrefs.GetFloat($"{playerPrefKey}/0", sensitivity);
            tolerance = PlayerPrefs.GetFloat($"{playerPrefKey}/1", tolerance);
        }
    }
}
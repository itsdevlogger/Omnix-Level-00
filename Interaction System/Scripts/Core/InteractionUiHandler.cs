using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionUiHandler : MonoBehaviour
    {
        public static InteractionUiHandler Instance { get; private set; }

        [SerializeField] private GameObject _keyHud;
        [SerializeField] private TextMeshProUGUI _keyLabel;

        [SerializeField] private GameObject _errorHud;
        [SerializeField] private TextMeshProUGUI _errorLabel;
        [SerializeField] private float _errorAutoHideDiration = 3f;

        private string _errorText;
        private int _blockers = 0;
        private Coroutine _errorHideCr;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                HideInfo();
                HideError();
            }
            else
            {
                Debug.LogError($"There are multiple {nameof(InteractionUiHandler)}s in this scene.");
                Destroy(gameObject);
            }
        }

        public static void AddBlocker()
        {
            Instance._blockers++;
        }

        public static void RemoveBlocker()
        {
            if (Instance._blockers > 0)
                Instance._blockers--;
        }

        public static void DisplayInfo(string label)
        {
            if (Instance._blockers > 0) return;
            if (string.IsNullOrEmpty(label)) return;

            Instance._keyHud.SetActive(true);
            Instance._keyLabel.text = label;
        }

        public static void DisplayError(string error)
        {
            if (Instance._blockers > 0) return;
            if (string.IsNullOrEmpty(error)) return;
            if (Instance._errorText == error) return;

            Instance._errorText = error;
            Instance._errorHud.SetActive(true);
            Instance._errorLabel.text = error;
            
            if (Instance._errorHideCr != null) 
                Instance.StopCoroutine(Instance._errorHideCr);
            Instance._errorHideCr = Instance.StartCoroutine(HideErrorCr());
        }

        private static IEnumerator HideErrorCr()
        {
            yield return new WaitForSeconds(Instance._errorAutoHideDiration);

            HideError();
        }

        public static void HideInfo()
        {
            Instance._keyHud.SetActive(false);
        }

        public static void HideError()
        {
            if (string.IsNullOrEmpty(Instance._errorText)) return;

            Instance._errorText = null;
            Instance._errorHideCr = null;
            Instance._errorHud.SetActive(false);
        }
    }
}

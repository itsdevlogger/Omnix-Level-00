using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Interactables
{
    [RequireComponent(typeof(Collider), typeof(IrnCooldown))]
    public class IrnDoor : MonoBehaviour, IInteractionProcessor
    {
        [Header("Referances")]
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _openPosition;
        [SerializeField] private Transform _closePosition;

        [Header("Animation Settings")]
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float _animationDuration;
        [SerializeField] private bool _copyPosition = false;
        [SerializeField] private bool _copyRotation = false;

        [Header("Other Settings")]
        [Tooltip("< 0 means door will stay open until interaction is ended, >= 0 means door will close automatically after that amount of time, and interaction will end instantly")]
        [SerializeField] private float _autocloseDelay;
        [SerializeField] private bool _isOpen = false;

        private Coroutine _currentAnimation;
        private IrnCooldown _cooldown;

        public bool IsAutoclose => _autocloseDelay >= 0;

        public float AnimationDuration
        {
            get => _animationDuration;
            set
            {
                _animationDuration = value;
                UpdateCooldown();
            }
        }

        public float AutocloseDelay
        {
            get => _autocloseDelay;
            set
            {
                _autocloseDelay = value;
                UpdateCooldown();
            }
        }

        private void Start()
        {
            _cooldown = GetComponent<IrnCooldown>();
            AnimationDuration = _animationDuration;

            // Set initial state
            Transform source = _isOpen ? _openPosition : _closePosition;
            if (_copyPosition) _target.localPosition = source.localPosition;
            if (_copyRotation) _target.localRotation = source.localRotation;
        }

        private void Reset()
        {
            _openPosition = new GameObject("Opened Position").transform;
            _closePosition = new GameObject("Closed Position").transform;
            _openPosition.SetParent(transform);
            _closePosition.SetParent(transform);
            _openPosition.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _closePosition.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (_cooldown ==  null) 
                _cooldown = GetComponent<IrnCooldown>();

            UpdateCooldown();
            EditorUtility.SetDirty(_cooldown);
#endif
        }

        private void UpdateCooldown()
        {
            if (IsAutoclose) _cooldown.cooldownTime = _animationDuration * 2f + _autocloseDelay;
            else _cooldown.cooldownTime = 0f;
        }

        public void OnInteractionStart(InteractionPlayer player)
        {
            if (_cooldown.IsOnCooldown)
                return;

            if (IsAutoclose)
            {
                if (_currentAnimation == null)
                    _currentAnimation = StartCoroutine(AnimateDoor(!_isOpen)); 
                else
                    _isOpen = !_isOpen;

                player.EndInteraction();
            }
            else if (!_isOpen)
            {
                if (_currentAnimation == null)
                    _currentAnimation = StartCoroutine(AnimateDoor(true));
                else
                    _isOpen = true;
            }
        }

        public void OnInteractionEnd(InteractionPlayer player)
        {
            if (IsAutoclose)
                return;

            if (_isOpen) 
            {
                if (_currentAnimation == null)
                    _currentAnimation = StartCoroutine(AnimateDoor(false));
                else
                    _isOpen = false;
            }
        }

        private IEnumerator AnimateDoor(bool open)
        {
            while (true)    // sh*t just got real yeh
            {
                _isOpen = open;
                Vector3 startPosition = _target.localPosition;
                Quaternion startRotation = _target.localRotation;
                Vector3 endPosition = open ? _openPosition.localPosition : _closePosition.localPosition;
                Quaternion endRotation = open ? _openPosition.localRotation : _closePosition.localRotation;

                float elapsedTime = 0f;
                while (elapsedTime < _animationDuration)
                {
                    float t = elapsedTime / _animationDuration;
                    float curveValue = _animationCurve.Evaluate(t);

                    if (_copyPosition) _target.localPosition = Vector3.Lerp(startPosition, endPosition, curveValue);
                    if (_copyRotation) _target.localRotation = Quaternion.Slerp(startRotation, endRotation, curveValue);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure final pos & rot
                if (_copyPosition) _target.localPosition = endPosition;
                if (_copyRotation) _target.localRotation = endRotation;

                if (_isOpen != open)
                {
                    // Player changed state of the during animation
                    // Run the loop again with new setup
                    open = _isOpen;
                }
                else if (_isOpen && IsAutoclose)
                {
                    yield return new WaitForSeconds(_autocloseDelay); // Hodor
                    open = false; // close door & run again
                }
                else
                {
                    // if nothing... end loop
                    _currentAnimation = null;
                    break;
                }
            }
        }
    }
}
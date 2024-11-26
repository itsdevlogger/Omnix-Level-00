using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Interactables
{
    [RequireComponent(typeof(IrnCooldown))]
    public class IrnDoor : MonoBehaviour, IInteractionProcessor
    {
        public Transform target;
        public Transform openPosition;
        public Transform closePosition;

        public AnimationCurve animationCurve;
        public float animationDuration;
        public bool copyPosition = false;
        public bool copyRotation = false;

        [Tooltip("< 0 means door will stay open until interaction is ended, >= 0 means door will close automatically after that amount of time, and interaction will end instantly")]
        [SerializeField] private float _autocloseDelay;
        [SerializeField] private bool _isOpen = false;

        private Coroutine _currentAnimation;
        private IrnCooldown _cooldown;

        public bool IsAutoclose => _autocloseDelay >= 0;

        public float AnimationDuration
        {
            get => animationDuration;
            set
            {
                animationDuration = value;
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
            AnimationDuration = animationDuration;

            // Set initial state
            Transform source = _isOpen ? openPosition : closePosition;
            if (copyPosition) target.localPosition = source.localPosition;
            if (copyRotation) target.localRotation = source.localRotation;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            openPosition = new GameObject("Opened Position").transform;
            closePosition = new GameObject("Closed Position").transform;
            openPosition.SetParent(transform);
            closePosition.SetParent(transform);
            openPosition.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            closePosition.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            _cooldown = GetComponent<IrnCooldown>();
            _cooldown.__EDITOR_ONLY_MANGED_BY__ = this;
        }

        private void OnValidate()
        {
            if (_cooldown ==  null) 
                _cooldown = GetComponent<IrnCooldown>();
            
            _cooldown.__EDITOR_ONLY_MANGED_BY__ = this;
            UpdateCooldown();
            EditorUtility.SetDirty(_cooldown);
        }

#endif
        private void UpdateCooldown()
        {
            if (IsAutoclose) _cooldown.cooldownTime = animationDuration * 2f + _autocloseDelay;
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
                Vector3 startPosition = target.localPosition;
                Quaternion startRotation = target.localRotation;
                Vector3 endPosition = open ? openPosition.localPosition : closePosition.localPosition;
                Quaternion endRotation = open ? openPosition.localRotation : closePosition.localRotation;

                float elapsedTime = 0f;
                while (elapsedTime < animationDuration)
                {
                    float t = elapsedTime / animationDuration;
                    float curveValue = animationCurve.Evaluate(t);

                    if (copyPosition) target.localPosition = Vector3.Lerp(startPosition, endPosition, curveValue);
                    if (copyRotation) target.localRotation = Quaternion.Slerp(startRotation, endRotation, curveValue);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure final pos & rot
                if (copyPosition) target.localPosition = endPosition;
                if (copyRotation) target.localRotation = endRotation;

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
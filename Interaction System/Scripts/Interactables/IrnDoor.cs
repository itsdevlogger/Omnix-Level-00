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
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _openPosition;
        [SerializeField] private Transform _closePosition;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _autocloseDelay;
        [SerializeField] private bool _isOpen = false;
        [SerializeField] private bool _copyPosition = false;
        [SerializeField] private bool _copyRotation = false;

        private Coroutine _currentAnimation;
        private IrnCooldown _cooldown;

        public float AnimationDuration
        {
            get => _animationDuration;
            set
            {
                _animationDuration = value;
                if (_autocloseDelay > 0) _cooldown.cooldownTime = value * 2f + _autocloseDelay;
                else  _cooldown.cooldownTime = value;
            }
        }

        public float AutocloseDelay
        {
            get => _autocloseDelay;
            set
            {
                _autocloseDelay = value;
                if (value > 0) _cooldown.cooldownTime = _animationDuration * 2f + value;
                else _cooldown.cooldownTime = _animationDuration;
            }
        }

        private void Start()
        {
            _cooldown = GetComponent<IrnCooldown>();
            AnimationDuration = _animationDuration;
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

            if (_autocloseDelay > 0) _cooldown.cooldownTime = _animationDuration * 2f + _autocloseDelay;
            else _cooldown.cooldownTime = _animationDuration;

            EditorUtility.SetDirty(_cooldown);
#endif
        }

        public void OnInteractionStart(InteractionPlayer player)
        {
            if (_cooldown.IsOnCooldown)
                return;

            if (_currentAnimation != null)
                StopCoroutine(_currentAnimation);

            _currentAnimation = StartCoroutine(AnimateDoor(!_isOpen));
            player.EndInteraction();
        }

        public void OnInteractionEnd(InteractionPlayer player)
        {
            
        }

        private IEnumerator AnimateDoor(bool open)
        {
            _isOpen = open;

            Vector3 startPosition = _target.position;
            Quaternion startRotation = _target.rotation;
            Vector3 endPosition = open ? _openPosition.position : _closePosition.position;
            Quaternion endRotation = open ? _openPosition.rotation : _closePosition.rotation;

            float elapsedTime = 0f;
            while (elapsedTime < _animationDuration)
            {
                float t = elapsedTime / _animationDuration;
                float curveValue = _animationCurve.Evaluate(t);

                if (_copyPosition) _target.position = Vector3.Lerp(startPosition, endPosition, curveValue);
                if (_copyRotation) _target.rotation = Quaternion.Slerp(startRotation, endRotation, curveValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final position and rotation are accurate
            if (_copyPosition) _target.position = endPosition;
            if (_copyRotation) _target.rotation = endRotation;

            if (_isOpen && _autocloseDelay > 0)
            {
                yield return new WaitForSeconds(_autocloseDelay);
                _currentAnimation = StartCoroutine(AnimateDoor(false));
            }
        }
    }
}
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Interactables
{
    [RequireComponent(typeof(CriCooldownTimer))]
    public class IrnDoor : MonoBehaviour, IInteractionProcessor
    {
        private delegate void ActionFrameSetter(float time, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation);

        public Transform target;
        public Transform openPosition;
        public Transform closePosition;

        public AnimationCurve animationCurve;
        public AnimationCurve interceptionCurve;
        public float animationDuration;
        [Tooltip("if true then the door will stop if interacted with during animations.")]
        public bool allowIntercept = true;
        public bool copyPosition = false;
        public bool copyRotation = false;


        [Tooltip("< 0 means door will stay open until interaction is ended, >= 0 means door will close automatically after that amount of time, and interaction will end instantly")]
        [SerializeField] private float _autocloseDelay;
        [SerializeField] private bool _isOpen = false;

        private Coroutine _currentAnimation;
        private CriCooldownTimer _cooldown;

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
            _cooldown = GetComponent<CriCooldownTimer>();
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

            _cooldown = GetComponent<CriCooldownTimer>();
            _cooldown.__EDITOR_ONLY_MANGED_BY__ = this;
        }

        private void OnValidate()
        {
            if (_cooldown ==  null) 
                _cooldown = GetComponent<CriCooldownTimer>();
            
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
                player.EndInteraction();
                StartAnimation(!_isOpen);
            }
            else if (!_isOpen)
            {
                StartAnimation(true);
            }
        }

        private void StartAnimation(bool open)
        {
            if (_currentAnimation == null)
            {
                _currentAnimation = StartCoroutine(AnimateDoor(open, GetFrameSetter()));
                return;
            }

            _isOpen = open;
        }

        public void OnInteractionEnd(InteractionPlayer player)
        {
            if (IsAutoclose)
                return;

            if (_isOpen) 
            {
                if (_currentAnimation == null)
                    _currentAnimation = StartCoroutine(AnimateDoor(false, GetFrameSetter()));
                else
                    _isOpen = false;
            }
        }

        private ActionFrameSetter GetFrameSetter()
        {
            if (copyPosition && copyRotation) return SetPositionAndRotationAtFrame;
            if (copyPosition) return SetPositionAtFrame;
            if (copyRotation) return SetRotationAtFrame;
            return SetPositionAndRotationAtFrame;
        }

        private void SetPositionAtFrame(float time, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation) 
        {
            target.localPosition = Vector3.Lerp(startPosition, endPosition, time);
        }

        private void SetRotationAtFrame(float time, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation)
        {
            target.localRotation = Quaternion.Slerp(startRotation, endRotation, time);
        }

        private void SetPositionAndRotationAtFrame(float time, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation)
        {
            target.localPosition = Vector3.Lerp(startPosition, endPosition, time);
            target.localRotation = Quaternion.Slerp(startRotation, endRotation, time);
        }

        private IEnumerator AnimateDoor(bool open, ActionFrameSetter frameSetter)
        {
            while (true)    // sh*t just got real yeh
            {
                _isOpen = open;
                Vector3 startPosition = target.localPosition;
                Quaternion startRotation = target.localRotation;
                Vector3 endPosition = open ? openPosition.localPosition : closePosition.localPosition;
                Quaternion endRotation = open ? openPosition.localRotation : closePosition.localRotation;

                float elapsedTime = 0f;
                bool intercepted = false;
                while (elapsedTime < animationDuration)
                {
                    float t = elapsedTime / animationDuration;
                    float curveValue = animationCurve.Evaluate(t);
                    frameSetter(curveValue, startPosition, endPosition, startRotation, endRotation);
                    
                    //if (copyPosition) target.localPosition = Vector3.Lerp(startPosition, endPosition, curveValue);
                    //if (copyRotation) target.localRotation = Quaternion.Slerp(startRotation, endRotation, curveValue);

                    elapsedTime += Time.deltaTime;

                    if (allowIntercept && _isOpen != open && _isOpen)
                    {
                        intercepted = true;
                        break;
                    }

                    yield return null;
                }

                if (intercepted)
                {
                    float time = 0f;
                    while (time < animationDuration / 10f)
                    {
                        elapsedTime += Time.deltaTime * interceptionCurve.Evaluate(time);
                        float curveValue = animationCurve.Evaluate(elapsedTime / animationDuration);
                        time += Time.deltaTime;
                        frameSetter(curveValue, startPosition, endPosition, startRotation, endRotation);
                        
                        //if (copyPosition) target.localPosition = Vector3.Lerp(startPosition, endPosition, curveValue);
                        //if (copyRotation) target.localRotation = Quaternion.Slerp(startRotation, endRotation, curveValue);
                        yield return null;
                    }

                    _currentAnimation = StartCoroutine(AnimateDoor(_isOpen, frameSetter));
                    yield break;
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
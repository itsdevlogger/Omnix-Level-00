using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace InteractionSystem
{
    public class RaycastHandler
    {
        public bool TargetChanged { get; private set; }
        public InteractionTarget Target { get; private set; }
        private Vector3 _lastRayOrigin;
        private Quaternion _lastRayAngle;

        private bool NeedRayCast(Ray ray)
        {
            Quaternion angle = Quaternion.LookRotation(ray.direction);
            if (Vector3.Distance(ray.origin, _lastRayOrigin) > 0.2f || Quaternion.Angle(angle, _lastRayAngle) > 1f)
            {
                _lastRayAngle = angle;
                _lastRayOrigin = ray.origin;
                return true;
            }
            return false;
        }

        /// <returns> true if we hit a target, false otherwiese </returns>
        public bool DoRaycast(float range, int layer, InteractionTarget currentTarget)
        {
            var ray = InteractionCamera.Instance.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!NeedRayCast(ray))
            {
                Target = currentTarget;
                TargetChanged = false;
                return Target != null;
            }

            if (Physics.Raycast(ray, out RaycastHit raycastHit, range, layer))
            {
                if (currentTarget != null && currentTarget.gameObject == raycastHit.collider.gameObject)
                {
                    TargetChanged = false;
                    Target = currentTarget;
                    return true;
                }

                if (InteractionTarget.TryGet(raycastHit.collider.gameObject, true, out var it))
                {
                    TargetChanged = true;
                    Target = it;
                    return true;
                }
            }
#if UNITY_EDITOR
            if (Physics.Raycast(ray, out raycastHit, range, ~layer))
            {
                var go = raycastHit.collider.gameObject;
                if (   go.TryGetComponent(out IInteractionCriteria _) 
                    || go.TryGetComponent(out IInteractionFeedback _)
                    || go.TryGetComponent(out IInteractionProcessor _)) 
                {
                    Debug.LogError($"Interactable \"{go.name}\" dont have Interactable Layer and will not be detected.");
                }
            }
#endif

                TargetChanged = true;
            Target = null;
            return false;
        }
    }

    public class InteractionPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _layer;

        [SerializeField, Tooltip("Maximum distance from which the player can start interaction")] 
        public float InteractionStartRange = 5f;
        
        [SerializeField, Tooltip("Force close interaction if player is further from this distance")]
        public float InteractionEndRange = 10f;

        [Header("Bindings")]
        [SerializeField] private InputAction _startInteractionAction;
        [SerializeField] private InputAction _endInteractionAction;

        private RaycastHandler _raycastHandler;
        public InteractionTarget CurrentTarget { get; private set; }
        public bool IsInteracting { get; private set; } = false;
        public bool IsFocused { get; private set; } = false;

        private void OnEnable()
        {
            _raycastHandler = new RaycastHandler();
            _startInteractionAction.Enable();
            _endInteractionAction.Enable();
        }

        private void OnDisable()
        {
            _startInteractionAction.Disable();
            _endInteractionAction.Disable();
        }

        private void Update()
        {
            if (IsInteracting)
            {
                bool shouldEndInteraction =
                        _endInteractionAction.IsPressed()
                    || CurrentTarget == null
                    || Vector3.Distance(transform.position, CurrentTarget.transform.position) >= InteractionEndRange;


                if (shouldEndInteraction) EndInteraction();
                return;
            }

            var hitAnything = _raycastHandler.DoRaycast(InteractionStartRange, _layer, CurrentTarget);
            if (_raycastHandler.TargetChanged)
            {
                ClearFocus();
                CurrentTarget = _raycastHandler.Target;
            }

            if (hitAnything)
            {
                if (IsFocused)
                {
                    if (_startInteractionAction.IsPressed())
                        StartInteraction();
                }
                else
                {
                    if (CurrentTarget.CanInteract(this))
                        SetFocus(CurrentTarget);
                }
            }
        }

        private void SetFocus(InteractionTarget target)
        {
            InteractionUiHandler.HideError();
            if (target == null) return;

            CurrentTarget = target;
            IsFocused = true;
            CurrentTarget.GainFocus(this);
        }


        /// <summary>
        /// Focus on the given interactable gameObject
        /// </summary>
        /// <param name="target"> Gameobject to focus on </param>
        /// <param name="force"> Should we force focus even if the <see cref="IInteractable.CanInteract(InteractionPlayer)"/> returns false? </param>
        public bool SetFocus(GameObject target, bool force)
        {
            if (!InteractionTarget.TryGet(target, true, out var ct))
                return false;

            if (!force && !ct.CanInteract(this))
                return false;

            if (CurrentTarget != null)
            {
                if (IsInteracting) EndInteraction();
                else ClearFocus();
            }

            CurrentTarget = ct;
            IsInteracting = true;
            CurrentTarget.StartInteraction(this);
            return true;
        }

        public void ClearFocus()
        {
            InteractionUiHandler.HideError();
            if (CurrentTarget == null || IsInteracting || IsFocused == false) 
                return;

            IsFocused = false;
            CurrentTarget.LooseFocus(this);
            CurrentTarget = null;
        }

        /// <summary>
        /// Start interaction with the currently focused target
        /// </summary>
        public void StartInteraction()
        {
            if (CurrentTarget == null || IsFocused == false) 
                return;

            IsInteracting = true; // Must be set before calling StartInteraction, incase Interaction immedietly ends
            CurrentTarget.StartInteraction(this);
        }

        /// <summary>
        /// Start interaction with the given interactable gameObject
        /// </summary>
        /// <param name="target"> Gameobject to interact with </param>
        /// <param name="force"> Should we force start interaction even if the <see cref="IInteractable.CanInteract(InteractionPlayer)"/> returns false? </param>
        public bool StartInteraction(GameObject target, bool force = false)
        {
            if (SetFocus(target, force))
            {
                CurrentTarget.StartInteraction(this);
                return true;
            }

            return false;
        }

        /// <summary>
        /// End current interaction
        /// </summary>
        public void EndInteraction()
        {
            if (CurrentTarget == null) return;

            IsInteracting = false;
            CurrentTarget.EndInteraction(this);
            ClearFocus();
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public class RaycastHandler
    {
        public bool TargetChanged { get; private set; }
        public InteractionTarget Target { get; private set; }
        public int interactionLayer;
        public int opaqueLayer;
        public float range;

        private Vector3 _lastRayOrigin;
        private Quaternion _lastRayAngle;

        private static Vector2 MousePosition
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Mouse.current.position.ReadValue();
#else
                return new Vector2(Screen.width / 2f, Screen.height / 2f);
#endif
            }
        }

        public RaycastHandler(int interactionLayer, int opaqueLayer, float range)
        {
            this.interactionLayer = interactionLayer;
            this.opaqueLayer = opaqueLayer;
            this.range = range;
        }

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
        public bool DoRaycast(InteractionTarget currentTarget)
        {
            var ray = InteractionCamera.Instance.ScreenPointToRay(MousePosition);
            if (!NeedRayCast(ray))
            {
                Target = currentTarget;
                TargetChanged = false;
                return Target != null;
            }

            if (Physics.Raycast(ray, out RaycastHit raycastHit, range, interactionLayer))
            {
                // check if the view is blocked
                if (Physics.Raycast(ray, raycastHit.distance, opaqueLayer))
                {
                    Target = null;
                    TargetChanged = currentTarget != null;
                    return false;
                }

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
            if (InteractionPlayer.EditorOnlyLogIncorrectlySetupInteractions &&  Physics.Raycast(ray, out raycastHit, range, ~interactionLayer))
            {
                var go = raycastHit.collider.gameObject;
                if (go.TryGetComponent(out IInteractionCriteria _)
                    || go.TryGetComponent(out IInteractionFeedback _)
                    || go.TryGetComponent(out IInteractable _))
                {
                    Debug.LogError($"Interactable \"{go.name}\" dont have Interactable Layer and will not be detected.");
                }
            }
#endif

            TargetChanged = currentTarget != null;
            Target = null;
            return false;
        }
    }
}

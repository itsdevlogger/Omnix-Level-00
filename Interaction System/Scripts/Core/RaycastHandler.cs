using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public class RaycastHandler
    {
        public bool TargetChanged { get; private set; }
        public InteractionTarget Target { get; private set; }
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
            var ray = InteractionCamera.Instance.ScreenPointToRay(MousePosition);
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
                if (go.TryGetComponent(out IInteractionCriteria _)
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
}

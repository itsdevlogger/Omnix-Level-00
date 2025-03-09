using UnityEngine;

namespace Omnix.Damping.Utils
{
    public class SmoothAim : BaseSmoothFollower
    {
        public enum WorldUpType
        {
            SceneUp = 0,
            ObjectUp = 1,
            ObjectRotationUp = 2,
            Vector = 3,
            None = 4
        }

        // @formatter:off
        [Header("Aim Settings")] 
        [Tooltip("The local axis that will point toward the target direction.")]
        [SerializeField] private Vector3 _aimVector = Vector3.forward;

        [Tooltip("The local up axis used for additional orientation correction.")]
        [SerializeField] private Vector3 _upVector = Vector3.up;

        [SerializeField] private WorldUpType _worldUpType = WorldUpType.SceneUp;

        [Tooltip("For ObjectUp or ObjectRotationUp types: assign a reference object.")]
        [SerializeField] private Transform _worldUpObject;

        [Tooltip("For Vector worldUpType: specify the desired up vector.")]
        [SerializeField]
        private Vector3 _worldUpVector = Vector3.up;
        // @formatter:on

        protected override Vector3 GetValue(Transform source) => (source.position - transform.position).normalized;

        protected override void SetValue(Vector3 value)
        {
            Vector3 computedWorldUp = _worldUpType switch
            {
                WorldUpType.SceneUp => Vector3.up,
                WorldUpType.ObjectUp => _worldUpObject ? _worldUpObject.up : Vector3.up,
                WorldUpType.ObjectRotationUp => _worldUpObject ? _worldUpObject.rotation * _upVector : Vector3.up,
                WorldUpType.Vector => _worldUpVector,
                WorldUpType.None => Vector3.zero,
                _ => Vector3.up
            };

            // First, compute the rotation that would align the local aimVector to the blended direction.
            Quaternion aimRotation = Quaternion.FromToRotation(_aimVector, value);
            // Get where the local upVector ends up after this rotation.
            Vector3 rotatedUp = aimRotation * _upVector;

            // If a world up is defined, adjust the rotation around the blended direction so that
            // the rotated up vector best aligns with the computed world up.
            if (computedWorldUp != Vector3.zero)
            {
                // Project both vectors onto the plane perpendicular to the blended direction.
                Vector3 projectedRotatedUp = Vector3.ProjectOnPlane(rotatedUp, value).normalized;
                Vector3 projectedWorldUp = Vector3.ProjectOnPlane(computedWorldUp, value).normalized;

                if (projectedRotatedUp.sqrMagnitude > 0 && projectedWorldUp.sqrMagnitude > 0)
                {
                    // Find the correction rotation about the blended direction.
                    Quaternion upCorrection = Quaternion.FromToRotation(projectedRotatedUp, projectedWorldUp);
                    Quaternion finalRotation = upCorrection * aimRotation;
                    transform.rotation = finalRotation;
                    return;
                }
            }

            // projections failed... Fallback
            transform.rotation = aimRotation;
        }
    }
}
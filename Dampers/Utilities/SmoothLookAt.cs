using UnityEngine;

namespace Omnix.Damping.Utils
{
    public class SmoothLookAt : BaseSmoothFollower
    {
        // @formatter:off
        [Header("Look At Settings")] 
        [SerializeField] private bool _useWorldUp = false;

        [Tooltip("Used when useWorldUp is false – defines a roll (in degrees) about the look direction.")]
        [SerializeField] private float _roll = 0f;

        [Tooltip("Used when WorldUp is true, this transform’s up vector is used as the up reference.")]
        [SerializeField] private Transform _worldUpObject;
        // @formatter:on

        protected override Vector3 GetValue(Transform source)
        {
            return source.position;
        }

        protected override void SetValue(Vector3 value)
        {
            Vector3 direction = value - transform.position;
            if (Mathf.Approximately(direction.sqrMagnitude, 0f))
                return;

            Vector3 upVector;
            if (_useWorldUp) upVector = (_worldUpObject != null) ? _worldUpObject.up : Vector3.up;
            else upVector = Quaternion.AngleAxis(_roll, direction.normalized) * Vector3.up;
            transform.rotation = Quaternion.LookRotation(direction, upVector);
        }
    }
}
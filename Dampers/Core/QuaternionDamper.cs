using System;
using UnityEngine;

namespace Omnix.Damping
{
    [Serializable]
    public class QuaternionDamper : BaseDamper<Quaternion, Vector3>
    {
        public QuaternionDamper(float frequency, float damping, float initialResponse) : base(frequency, damping,
            initialResponse)
        {
        }

        protected override Vector3 GetVelocity(Quaternion target, float dt)
        {
            Quaternion difference = target * Quaternion.Inverse(lastTarget);
            difference.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f) angle -= 360f;
            return angle * Mathf.Deg2Rad * axis / dt;
        }

        protected override void Increment(Quaternion target, Vector3 targetVelocity, float dt, float k2Safe)
        {
            Quaternion velocityRotation =
                Quaternion.AngleAxis(velocity.magnitude * Mathf.Rad2Deg * dt, velocity.normalized);
            lastOut = velocityRotation * lastOut;

            // Calculate error quaternion
            Quaternion error = target * Quaternion.Inverse(lastOut);
            error.ToAngleAxis(out float errorAngle, out Vector3 errorAxis);
            if (errorAngle > 180f)
                errorAngle -= 360f;
            Vector3 errorVector = errorAxis * (errorAngle * Mathf.Deg2Rad);

            // Update velocity
            velocity += dt * (errorVector + k3 * targetVelocity - k1 * velocity) / k2Safe;
        }
    }
}
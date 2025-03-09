using System;
using UnityEngine;

namespace Omnix.Damping
{
    [Serializable]
    public class Vector3Damper : BaseDamper<Vector3, Vector3>
    {
        public Vector3Damper(float frequency, float damping, float initialResponse) : base(frequency, damping, initialResponse)
        {
        }

        protected override Vector3 GetVelocity(Vector3 target, float dt)
        {
            return (target - lastTarget) / dt;
        }

        protected override void Increment(Vector3 target, Vector3 targetVelocity, float dt, float k2Safe)
        {
            lastOut += dt * velocity;
            velocity += dt * (target + k3 * targetVelocity - lastOut - k1 * velocity) / k2Safe;
        }
    }
}
using System;
using UnityEngine;

namespace Omnix.Damping
{
    [Serializable]
    public class Vector2Damper : BaseDamper<Vector2, Vector2>
    {
        public Vector2Damper(float frequency, float damping, float initialResponse) : base(frequency, damping,
            initialResponse)
        {
        }

        protected override Vector2 GetVelocity(Vector2 target, float dt)
        {
            return (target - lastTarget) / dt;
        }

        protected override void Increment(Vector2 target, Vector2 targetVelocity, float dt, float k2Safe)
        {
            lastOut += dt * velocity;
            velocity += dt * (target + k3 * targetVelocity - lastOut - k1 * velocity) / k2Safe;
        }
    }
}
using System;

namespace Omnix.Damping
{
    [Serializable]
    public class FloatDamper : BaseDamper<float, float>
    {
        public FloatDamper(float frequency, float damping, float initialResponse) : base(frequency, damping,
            initialResponse)
        {
        }

        protected override float GetVelocity(float target, float dt)
        {
            return (target - lastTarget) / dt;
        }

        protected override void Increment(float target, float targetVelocity, float dt, float k2Safe)
        {
            lastOut += dt * velocity;
            velocity += dt * (target + k3 * targetVelocity - lastOut - k1 * velocity) / k2Safe;
        }
    }
}
using System;
using UnityEngine;

namespace Omnix.Damping
{
    [Serializable]
    public class ColorDamper : BaseDamper<Color, Vector4>
    {
        public ColorDamper(float frequency, float damping, float initialResponse) : base(frequency, damping, initialResponse)
        {
        }

        protected override Vector4 GetVelocity(Color target, float dt)
        {
            return (new Vector4(target.r, target.g, target.b, target.a) -
                    new Vector4(lastTarget.r, lastTarget.g, lastTarget.b, lastTarget.a)) / dt;
        }

        protected override void Increment(Color target, Vector4 targetVelocity, float dt, float k2Safe)
        {
            Vector4 lastOutVec = new Vector4(lastOut.r, lastOut.g, lastOut.b, lastOut.a);
            Vector4 velocityVec = new Vector4(velocity.x, velocity.y, velocity.z, velocity.w);
        
            lastOutVec += dt * velocityVec;
            velocityVec += dt * (new Vector4(target.r, target.g, target.b, target.a) + k3 * targetVelocity - lastOutVec - k1 * velocityVec) / k2Safe;
        
            lastOut = new Color(lastOutVec.x, lastOutVec.y, lastOutVec.z, lastOutVec.w);
            velocity = velocityVec;
        }
    }
}
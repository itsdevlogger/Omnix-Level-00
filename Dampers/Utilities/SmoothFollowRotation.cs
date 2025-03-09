using UnityEngine;

namespace Omnix.Damping.Utils
{
    public class SmoothFollowRotation : BaseSmoothFollower
    {
        protected override Vector3 GetValue(Transform source) => source.rotation.eulerAngles;
        protected override void SetValue(Vector3 value) => transform.rotation = Quaternion.Euler(value);
    }
}
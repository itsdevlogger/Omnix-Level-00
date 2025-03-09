using UnityEngine;

namespace Omnix.Damping.Utils
{
    public class SmoothFollowPosition : BaseSmoothFollower
    {
        protected override Vector3 GetValue(Transform source) => source.position;
        protected override void SetValue(Vector3 value) => transform.position = value;
    }
}
using UnityEngine;

namespace Omnix.Damping.Utils
{
    public class SmoothFollowScale : BaseSmoothFollower
    {
        protected override Vector3 GetValue(Transform source) => source.localScale;
        protected override void SetValue(Vector3 value) => transform.localScale = value;
    }
}
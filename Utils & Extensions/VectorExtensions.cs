using Omnix.Utils;
using UnityEngine;

namespace Omnix.Extensions
{
    public static class VectorExtensions
    {
        #region Vector3
        public static Vector3 Random(Vector3 min, Vector3 max) => new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
        public static float Distance(this Vector3 v1, Vector3 v2) => Vector3.Distance(v1, v2);
        public static float SquareDistance(this Vector3 v1, Vector3 v2) => Vector3.SqrMagnitude(v1 - v2);
        public static Vector3 Dot(this Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        public static Vector3 InverseProduct(this Vector3 v1, Vector3 v2) => new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        public static Vector3 Rounded(this Vector3 value, Vector3 stepSize) => new Vector3(NumberUtils.Round(value.x, stepSize.x), NumberUtils.Round(value.y, stepSize.y), NumberUtils.Round(value.z, stepSize.z));
        #endregion

        #region Vector2
        public static Vector2 Random(Vector2 min, Vector2 max) => new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
        public static float Distance(this Vector2 v1, Vector2 v2) => Vector2.Distance(v1, v2);
        public static float SquareDistance(this Vector2 v1, Vector2 v2) => Vector2.SqrMagnitude(v1 - v2);
        public static Vector2 Dot(this Vector2 v1, Vector2 v2) => new Vector2(v1.x * v2.x, v1.y * v2.y);
        public static Vector2 Divide(this Vector2 v1, Vector2 v2) => new Vector2(v1.x / v2.x, v1.y / v2.y);
        public static Vector2 Rounded(this Vector2 value, Vector2 stepSize) => new Vector2(NumberUtils.Round(value.x, stepSize.x), NumberUtils.Round(value.y, stepSize.y));
        #endregion
    }
}
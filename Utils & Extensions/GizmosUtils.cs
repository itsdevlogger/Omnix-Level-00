using UnityEngine;

namespace Omnix.Utils
{
    public static class GizmosUtils
    {
        private static readonly Quaternion RightQuaternion = new Quaternion(0.0f, 1.0f, 0.0f, -0.3f);
        private static readonly Quaternion LeftQuaternion = new Quaternion(0.0f, 1.0f, 0.0f, 0.3f);
        private static readonly Quaternion UpQuaternion = new Quaternion(1.0f, 0.0f, 0.0f, -0.3f);
        private static readonly Quaternion DownQuaternion = new Quaternion(1.0f, 0.0f, 0.0f, 0.3f);


        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="resolution"></param>
        public static void DrawCone2(Vector3 origin, Vector3 direction, float depth, float baseRadius, int resolution)
        {
            Quaternion rotator = Quaternion.Euler(direction * (360f / resolution));
            Vector3 normal = Vector3.Cross(direction, Vector3.up).normalized;
            Vector3 point2 = origin + (direction * depth);
            Vector3 last = Vector3.zero;
            ;
            Vector3 current;
            ;

            for (int i = 0; i < resolution + 1; i++)
            {
                current = point2 + (normal * baseRadius);
                if (i > 0) Gizmos.DrawLine(last, current);
                last = current;
                Gizmos.DrawLine(origin, current);
                Gizmos.DrawLine(point2, current);
                normal = rotator * normal;
            }
        }

        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="resolution"></param>
        /// <param name="color"></param>
        public static void DrawCone2(Vector3 origin, Vector3 direction, float depth, float baseRadius, int resolution, Color color)
        {
            Color old = Gizmos.color;
            Gizmos.color = color;
            DrawCone2(origin, direction, depth, baseRadius, resolution);
            Gizmos.color = old;
        }

        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians) that the cone's slope makes with the zero point </param>
        /// <param name="resolution"> </param>
        public static void DrawCone(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, int resolution)
        {
            DrawCone2(origin, direction, depth, Mathf.Tan(halfConeAngle) * depth, resolution);
        }

        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians) that the cone's slope makes with the zero point </param>
        /// <param name="resolution"> </param>
        /// <param name="color"></param>
        public static void DrawCone(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, int resolution, Color color)
        {
            Color old = Gizmos.color;
            Gizmos.color = color;
            DrawCone2(origin, direction, depth, Mathf.Tan(halfConeAngle) * depth, resolution);
            Gizmos.color = old;
        }
        
        public static void DrawArrow(Vector3 origin, Vector3 direction, Color color)
        {
            Color old = Gizmos.color;
            Gizmos.color = color;
            DrawArrow(origin, direction);
            Gizmos.color = old;
        }

        public static void DrawArrow(Vector3 origin, Vector3 direction)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 right = lookRotation * RightQuaternion * new Vector3(0, 0, 1);
            Vector3 left = lookRotation * LeftQuaternion * new Vector3(0, 0, 1);
            Vector3 up = lookRotation * UpQuaternion * new Vector3(0, 0, 1);
            Vector3 down = lookRotation * DownQuaternion * new Vector3(0, 0, 1);

            Gizmos.DrawRay(origin, direction);
            Gizmos.DrawRay(origin + direction, right * 0.25f);
            Gizmos.DrawRay(origin + direction, left * 0.25f);
            Gizmos.DrawRay(origin + direction, up * 0.25f);
            Gizmos.DrawRay(origin + direction, down * 0.25f);
        }
    }
}
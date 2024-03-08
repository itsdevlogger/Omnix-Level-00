using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omnix.Utils
{
    public static class PhysicsUtils
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="depth"></param>
        /// <param name="coneAngle"> in degrees</param>
        /// <returns></returns>
        private static bool IsObjectWithinCone(Collider c, Vector3 origin, Vector3 direction, float depth, float coneAngle)
        {
            Vector3 toObject = c.transform.position - origin;

            // Check if the distance from object to origin is within range
            if (Vector3.Dot(toObject, direction) > depth)
            {
                return false;
            }

            // Check the angle within range
            float angleToHit = Vector3.Angle(direction, toObject);
            return angleToHit < coneAngle;
        }

        private static Collider[] ArrangeArrayAlloc(Collider[] sphereCastHits, Vector3 origin, Vector3 direction, float depth, float halfConeAngle)
        {
            if (sphereCastHits.Length <= 0) return Array.Empty<Collider>();

            List<Collider> coneCastHitList = new List<Collider>();
            float angleInDegrees = halfConeAngle * Mathf.Rad2Deg;
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (IsObjectWithinCone(sphereCastHits[i], origin, direction, depth, angleInDegrees))
                {
                    coneCastHitList.Add(sphereCastHits[i]);
                }
            }

            Collider[] array = new Collider[coneCastHitList.Count];
            for (int i = 0; i < coneCastHitList.Count; i++)
            {
                array[i] = coneCastHitList[i];
            }

            return array;
        }

        private static int ArrangeArrayNonAlloc(Collider[] sphereCastHits, int sphereCount, Vector3 origin, Vector3 direction, float depth, float halfConeAngle)
        {
            if (sphereCount == 0) return 0;

            int count = 0;
            float angleInDegrees = halfConeAngle * Mathf.Rad2Deg;
            for (int i = 0; i < sphereCount; i++)
            {
                Collider c = sphereCastHits[i];
                if (IsObjectWithinCone(c, origin, direction, depth, angleInDegrees) == false) continue;
                (sphereCastHits[i], sphereCastHits[count]) = (sphereCastHits[count], sphereCastHits[i]);
                count++;
            }

            return count;
        }

        private static void FromConeToCapsule(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius)
        {
            radius = depth * Mathf.Tan(halfConeAngle);
            point0 = origin + direction * (depth - radius);
            point1 = origin + direction * radius;
        }
        

        /// <summary> Creates an imaginary cone. </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> (Normalized) Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians, between 0 to PI/2) that the cone's slope makes with the zero point </param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> All the colliders within provided cone </returns>
        [Obsolete("Use OverlapConeNonAlloc to save some memory and power")]
        public static Collider[] OverlapCone(Vector3 origin, Vector3 direction, float depth, float halfConeAngle)
        {
            FromConeToCapsule(origin, direction, depth, halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius);
            Collider[] sphereCastHits = Physics.OverlapCapsule(point0, point1, radius);
            return ArrangeArrayAlloc(sphereCastHits, origin, direction, depth, halfConeAngle);
        }

        /// <summary> Creates an imaginary cone. </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> (Normalized) Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians, between 0 to PI/2) that the cone's slope makes with the zero point </param>
        /// <param name="layerMask"> LayerMask </param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> All the colliders within provided cone </returns>
        [Obsolete("Use OverlapConeNonAlloc to save some memory and power")]
        public static Collider[] OverlapCone(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, int layerMask)
        {
            FromConeToCapsule(origin, direction, depth, halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius);
            Collider[] sphereCastHits = Physics.OverlapCapsule(point0, point1, radius, layerMask);
            return ArrangeArrayAlloc(sphereCastHits, origin, direction, depth, halfConeAngle);
        }

        /// <summary> Creates an imaginary cone. </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> (Normalized) Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians, between 0 to PI/2) that the cone's slope makes with the zero point </param>
        /// <param name="layerMask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> All the colliders within provided cone </returns>
        [Obsolete("Use OverlapConeNonAlloc to save some memory and power")]
        public static Collider[] OverlapCone(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            FromConeToCapsule(origin, direction, depth, halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius);
            Collider[] sphereCastHits = Physics.OverlapCapsule(point0, point1, radius, layerMask, queryTriggerInteraction);
            return ArrangeArrayAlloc(sphereCastHits, origin, direction, depth, halfConeAngle);
        }

        /// <summary>
        /// Creates an imaginary cone. But in an non-allocate way
        /// Does not attempt to grow the buffer if it runs out of space. The length of the buffer is returned when the buffer is full
        /// </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians, between 0 to PI/2) that the cone's slope makes with the zero point </param>
        /// <param name="results"> Buffer array. </param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> number of colliders stored into the results buffer. </returns>
        public static int OverlapConeNonAlloc(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, Collider[] results)
        {
            FromConeToCapsule(origin, direction, depth, halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius);
            int sphereCount = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results);
            return ArrangeArrayNonAlloc(results, sphereCount, origin, direction, depth, halfConeAngle);
        }

        /// <summary>
        /// Creates an imaginary cone. But in an non-allocate way
        /// Does not attempt to grow the buffer if it runs out of space. The length of the buffer is returned when the buffer is full
        /// </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians, between 0 to PI/2) that the cone's slope makes with the zero point </param>
        /// <param name="results"> Buffer array. </param>
        /// <param name="layerMask"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> number of colliders stored into the results buffer. </returns>
        public static int OverlapConeNonAlloc(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, Collider[] results, int layerMask)
        {
            FromConeToCapsule(origin, direction, depth, halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius);
            int sphereCount = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask);
            return ArrangeArrayNonAlloc(results, sphereCount, origin, direction, depth, halfConeAngle);
        }

        /// <summary>
        /// Creates an imaginary cone. But in an non-allocate way
        /// Does not attempt to grow the buffer if it runs out of space. The length of the buffer is returned when the buffer is full
        /// </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="halfConeAngle"> Angle (in radians, between 0 to PI/2) that the cone's slope makes with the zero point </param>
        /// <param name="results"> Buffer array. </param>
        /// <param name="layerMask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> number of colliders stored into the results buffer. </returns>
        public static int OverlapConeNonAlloc(Vector3 origin, Vector3 direction, float depth, float halfConeAngle, Collider[] results, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            FromConeToCapsule(origin, direction, depth, halfConeAngle, out Vector3 point0, out Vector3 point1, out float radius);
            int sphereCount = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, queryTriggerInteraction);
            return ArrangeArrayNonAlloc(results, sphereCount, origin, direction, depth, halfConeAngle);
        }


        /// <summary> Creates an imaginary cone. </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> All the colliders within provided cone </returns>
        [Obsolete("Use OverlapCone2NonAlloc to save some memory and power")]
        public static Collider[] OverlapCone2(Vector3 origin, Vector3 direction, float depth, float baseRadius)
        {
            return OverlapCone(origin, direction, depth, Mathf.Atan2(baseRadius, depth));
        }

        /// <summary> Creates an imaginary cone. </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="layerMask"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> All the colliders within provided cone </returns>
        [Obsolete("Use OverlapCone2NonAlloc to save some memory and power")]
        public static Collider[] OverlapCone2(Vector3 origin, Vector3 direction, float depth, float baseRadius, int layerMask)
        {
            return OverlapCone(origin, direction, depth, Mathf.Atan2(baseRadius, depth), layerMask);
        }

        /// <summary> Creates an imaginary cone. </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="layerMask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> All the colliders within provided cone </returns>
        [Obsolete("Use OverlapCone2NonAlloc to save some memory and power")]
        public static Collider[] OverlapCone2(Vector3 origin, Vector3 direction, float depth, float baseRadius, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            return OverlapCone(origin, direction, depth, Mathf.Atan2(baseRadius, depth), layerMask, queryTriggerInteraction);
        }

        /// <summary>
        /// Creates an imaginary cone. But in an non-allocate way
        /// Does not attempt to grow the buffer if it runs out of space. The length of the buffer is returned when the buffer is full
        /// </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="results"> Buffer array. </param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> number of colliders stored into the results buffer. </returns>
        public static int OverlapCone2NonAlloc(Vector3 origin, Vector3 direction, float depth, float baseRadius, Collider[] results)
        {
            return OverlapConeNonAlloc(origin, direction, depth, Mathf.Atan2(baseRadius, depth), results);
        }

        /// <summary>
        /// Creates an imaginary cone. But in an non-allocate way
        /// Does not attempt to grow the buffer if it runs out of space. The length of the buffer is returned when the buffer is full
        /// </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="results"> Buffer array. </param>
        /// <param name="layerMask"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> number of colliders stored into the results buffer. </returns>
        public static int OverlapCone2NonAlloc(Vector3 origin, Vector3 direction, float depth, float baseRadius, Collider[] results, int layerMask)
        {
            return OverlapConeNonAlloc(origin, direction, depth, Mathf.Atan2(baseRadius, depth), results, layerMask);
        }

        /// <summary>
        /// Creates an imaginary cone. But in an non-allocate way
        /// Does not attempt to grow the buffer if it runs out of space. The length of the buffer is returned when the buffer is full
        /// </summary>
        /// <param name="origin"> Zero point of the cone </param>
        /// <param name="direction"> Vector pointing in the direction of Base </param>
        /// <param name="depth"> Distance of the Base from the origin </param>
        /// <param name="baseRadius"> Radius of the base of the cone </param>
        /// <param name="results"> Buffer array. </param>
        /// <param name="layerMask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <remarks> Assumes that direction vector is normalized </remarks>
        /// <returns> number of colliders stored into the results buffer. </returns>
        public static int OverlapCone2NonAlloc(Vector3 origin, Vector3 direction, float depth, float baseRadius, Collider[] results, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            return OverlapConeNonAlloc(origin, direction, depth, Mathf.Atan2(baseRadius, depth), results, layerMask, queryTriggerInteraction);
        }


        /// <summary>
        /// Adds a force to this rigidbody to simulate it being attached via a spring.
        /// Assuming that the other end of the spring is not moving.
        /// </summary>
        public static void AddSpringForce(this Rigidbody rb, Vector3 springOrigin, float springConstant)
        {
            rb.AddForce((springOrigin - rb.position) * springConstant);
        }

        /// <summary>
        /// Adds a force to this rigidbody to simulate it being attached via a spring.
        /// Assuming that the other end of the spring is moving.
        /// </summary>
        /// <param name="originDelta"> How much did the origin move in this frame </param>
        /// <param name="springStiffness"> How much affect should the origin movement have on spring force being applied. 0 meaning no effect, 1 meaning 100% influence. </param>
        public static void AddSpringForce(this Rigidbody rb, Vector3 springOrigin, float springConstant, Vector3 originDelta, float springStiffness)
        {
            Vector3 springForce = (springOrigin - rb.position) * springConstant;
            rb.AddForce(springForce + originDelta * springConstant * springStiffness);
        }
        
        public static void AddSpringForce2(this Rigidbody rb, Vector3 springOrigin, float springConstant, Vector3 originDelta, float springStiffness)
        {
            // Extracted the logic for applying force on a single target
            Vector3 springForce = (springOrigin - rb.position) * springConstant;
            float totalForceMagnitude = springForce.magnitude;
            Vector3 springForceDirection = springForce.normalized;
            
            if (originDelta.magnitude > 0)
            {
                Vector3 movementDirection = originDelta.normalized;
                springForce = Vector3.SlerpUnclamped(springForceDirection, movementDirection, springStiffness);
                springForce *= totalForceMagnitude;
            }

            rb.AddForce(springForce);
        }

        /// <summary>
        /// Adds a force to this rigidbody to simulate it being under influence a magnetic field
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="sourcePosition"> Position of the source of the magnetic field. </param>
        /// <param name="rbCharge"> Charge present on the rigidbody </param>
        /// <param name="sourceCharge"> Charge present on the source of magnetic field </param>
        public static void AddMagneticForce(this Rigidbody rb, Vector3 sourcePosition, float sourceCharge = 1f, float rbCharge = 1f)
        {
            Vector3 magneticField = sourcePosition - rb.position;
            float forceMagnitude = sourceCharge * rbCharge / magneticField.sqrMagnitude;
            rb.AddForce(magneticField.normalized * forceMagnitude);
        }
    }
}
using UnityEngine;


namespace Omnix.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary> Specify the name for this gameObject </summary>
        public static T Name<T>(this T comp, string value) where T : Component
        {
            comp.name = value;
            return comp;
        }

        /// <summary> Specify the Parent for this gameObject </summary>
        public static T SetParent<T>(this T comp, Transform value) where T : Component
        {
            comp.transform.parent = value;
            return comp;
        }

        /// <summary> Specify the Global Position for this gameObject </summary>
        public static T Position<T>(this T comp, Vector3 value) where T : Component
        {
            comp.transform.position = value;
            return comp;
        }

        /// <summary> Specify the Local Position for this gameObject </summary>
        public static T LocalPosition<T>(this T comp, Vector3 value) where T : Component
        {
            comp.transform.localPosition = value;
            return comp;
        }

        /// <summary> Specify the Global Rotation for this gameObject </summary>
        public static T Rotation<T>(this T comp, Quaternion value) where T : Component
        {
            comp.transform.rotation = value;
            return comp;
        }

        /// <summary> Specify the Global Rotation for this gameObject </summary>
        public static T Rotation<T>(this T comp, Vector3 value) where T : Component
        {
            comp.transform.rotation = Quaternion.Euler(value);
            return comp;
        }

        /// <summary> Specify the Local Rotation for this gameObject </summary>
        public static T LocalRotation<T>(this T comp, Quaternion value) where T : Component
        {
            comp.transform.localRotation = value;
            return comp;
        }

        /// <summary> Specify the Local Rotation for this gameObject </summary>
        public static T LocalRotation<T>(this T comp, Vector3 value) where T : Component
        {
            comp.transform.localRotation = Quaternion.Euler(value);
            return comp;
        }

        /// <summary> Specify the Local Scale for this gameObject </summary>
        public static T LocalScale<T>(this T comp, Vector3 value) where T : Component
        {
            comp.transform.localScale = value;
            return comp;
        }
        
        /// <summary> Specify the Components to add on this gameObject </summary>
        public static T Components<T, T0>(this T comp)
        where T : MonoBehaviour
        where T0 : Component
        {
            comp.gameObject.AddComponent<T0>();
            return comp;
        }

        /// <summary> Specify the Components to add on this gameObject </summary>
        public static T Components<T, T0, T1>(this T comp)
        where T : MonoBehaviour
        where T0 : Component
        where T1 : Component
        {
            GameObject gameObj = comp.gameObject;
            gameObj.AddComponent<T0>();
            gameObj.AddComponent<T1>();
            return comp;
        }

        /// <summary> Specify the Components to add on this gameObject </summary>
        public static T Components<T, T0, T1, T2>(this T comp)
        where T : MonoBehaviour
        where T0 : Component
        where T1 : Component
        where T2 : Component
        {
            GameObject gameObj = comp.gameObject;
            gameObj.AddComponent<T0>();
            gameObj.AddComponent<T1>();
            gameObj.AddComponent<T2>();
            return comp;
        }

        /// <summary> Specify the Components to add on this gameObject </summary>
        public static T Components<T, T0, T1, T2, T3>(this T comp)
        where T : MonoBehaviour
        where T0 : Component
        where T1 : Component
        where T2 : Component
        where T3 : Component
        {
            GameObject gameObj = comp.gameObject;
            gameObj.AddComponent<T0>();
            gameObj.AddComponent<T1>();
            gameObj.AddComponent<T2>();
            gameObj.AddComponent<T3>();
            return comp;
        }

        /// <summary> Specify the Components to add on this gameObject </summary>
        public static T Components<T, T0, T1, T2, T3, T4>(this T comp)
        where T : MonoBehaviour
        where T0 : Component
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        {
            GameObject gameObj = comp.gameObject;
            gameObj.AddComponent<T0>();
            gameObj.AddComponent<T1>();
            gameObj.AddComponent<T2>();
            gameObj.AddComponent<T4>();
            return comp;
        }

        /// <summary> Specify the layer for this gameObject </summary>
        public static T Layer<T>(this T comp, int layerMask) where T : Component
        {
            comp.gameObject.layer = layerMask;
            return comp;
        }

        /// <summary> Specify the Tag for this gameObject </summary>
        public static T Tag<T>(this T comp, string tag) where T : Component
        {
            comp.gameObject.tag = tag;
            return comp;
        }

        /// <summary> Specify point to which this gameObject is looking </summary>
        public static T LookAt<T>(this T comp, Vector3 target) where T : Component
        {
            comp.transform.LookAt(target);
            return comp;
        }

        /// <summary> Specify object to which this gameObject is looking when this object is created</summary>
        public static T LookAt<T>(this T comp, Transform target) where T : Component
        {
            comp.transform.LookAt(target);
            return comp;
        }
    }
}
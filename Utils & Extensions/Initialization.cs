using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Omnix.Initialization
{
    public static class Initialization
    {

        #region Init Without Prefab
        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0>(T0 arg0) where T : Component, IInit<T0>
        {
            T go = new GameObject().AddComponent<T>();
            go.Init(arg0);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1>(T0 arg0, T1 arg1) where T : Component, IInit<T0, T1>
        {
            T go = new GameObject().AddComponent<T>();
            go.Init(arg0, arg1);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2) where T : Component, IInit<T0, T1, T2>
        {
            T go = new GameObject().AddComponent<T>();
            go.Init(arg0, arg1, arg2);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : Component, IInit<T0, T1, T2, T3>
        {
            T go = new GameObject().AddComponent<T>();
            go.Init(arg0, arg1, arg2, arg3);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : Component, IInit<T0, T1, T2, T3, T4>
        {
            T go = new GameObject().AddComponent<T>();
            go.Init(arg0, arg1, arg2, arg3, arg4);
            return go;
        }
        #endregion

        #region Init With Prefab
        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0>(T prefab, T0 arg0) where T : Component, IInit<T0>
        {
            T go = Object.Instantiate(prefab);
            go.Init(arg0);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1>(T prefab, T0 arg0, T1 arg1) where T : Component, IInit<T0, T1>
        {
            T go = Object.Instantiate(prefab);
            go.Init(arg0, arg1);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1, T2>(T prefab, T0 arg0, T1 arg1, T2 arg2) where T : Component, IInit<T0, T1, T2>
        {
            T go = Object.Instantiate(prefab);
            go.Init(arg0, arg1, arg2);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1, T2, T3>(T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : Component, IInit<T0, T1, T2, T3>
        {
            T go = Object.Instantiate(prefab);
            go.Init(arg0, arg1, arg2, arg3);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T Init<T, T0, T1, T2, T3, T4>(T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : Component, IInit<T0, T1, T2, T3, T4>
        {
            T go = Object.Instantiate(prefab);
            go.Init(arg0, arg1, arg2, arg3, arg4);
            return go;
        }
        #endregion

        #region Start Without Prefab
        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0>(T0 arg0) where T : MonoBehaviour, IStart<T0>
        {
            T go = new GameObject().AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1>(T0 arg0, T1 arg1) where T : MonoBehaviour, IStart<T0, T1>
        {
            T go = new GameObject().AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2) where T : MonoBehaviour, IStart<T0, T1, T2>
        {
            T go = new GameObject().AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : MonoBehaviour, IStart<T0, T1, T2, T3>
        {
            T go = new GameObject().AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : MonoBehaviour, IStart<T0, T1, T2, T3, T4>
        {
            T go = new GameObject().AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3, arg4));
            return go;
        }
        #endregion

        #region Start With Prefab
        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0>(T prefab, T0 arg0) where T : MonoBehaviour, IStart<T0>
        {
            T go = Object.Instantiate(prefab);
            go.StartCoroutine(go.OnStart(arg0));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1>(T prefab, T0 arg0, T1 arg1) where T : MonoBehaviour, IStart<T0, T1>
        {
            T go = Object.Instantiate(prefab);
            go.StartCoroutine(go.OnStart(arg0, arg1));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1, T2>(T prefab, T0 arg0, T1 arg1, T2 arg2) where T : MonoBehaviour, IStart<T0, T1, T2>
        {
            T go = Object.Instantiate(prefab);
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1, T2, T3>(T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : MonoBehaviour, IStart<T0, T1, T2, T3>
        {
            T go = Object.Instantiate(prefab);
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls OnStart Coroutine on it </summary>
        /// <returns> Created (& started) gameObject </returns>
        public static T Start<T, T0, T1, T2, T3, T4>(T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : MonoBehaviour, IStart<T0, T1, T2, T3, T4>
        {
            T go = Object.Instantiate(prefab);
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3, arg4));
            return go;
        }
        #endregion

        #region Init Component
        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T InitComponent<T, T0>(this GameObject gameObject, T0 arg0) where T : Component, IInit<T0>
        {
            T go = gameObject.AddComponent<T>();
            go.Init(arg0);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T InitComponent<T, T0, T1>(this GameObject gameObject, T0 arg0, T1 arg1) where T : Component, IInit<T0, T1>
        {
            T go = gameObject.AddComponent<T>();
            go.Init(arg0, arg1);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T InitComponent<T, T0, T1, T2>(this GameObject gameObject, T0 arg0, T1 arg1, T2 arg2) where T : Component, IInit<T0, T1, T2>
        {
            T go = gameObject.AddComponent<T>();
            go.Init(arg0, arg1, arg2);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T InitComponent<T, T0, T1, T2, T3>(this GameObject gameObject, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : Component, IInit<T0, T1, T2, T3>
        {
            T go = gameObject.AddComponent<T>();
            go.Init(arg0, arg1, arg2, arg3);
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T InitComponent<T, T0, T1, T2, T3, T4>(this GameObject gameObject, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : Component, IInit<T0, T1, T2, T3, T4>
        {
            T go = gameObject.AddComponent<T>();
            go.Init(arg0, arg1, arg2, arg3, arg4);
            return go;
        }
        #endregion

        #region Start Component
        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T StartComponent<T, T0>(this GameObject gameObject, T0 arg0) where T : MonoBehaviour, IStart<T0>
        {
            T go = gameObject.AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T StartComponent<T, T0, T1>(this GameObject gameObject, T0 arg0, T1 arg1) where T : MonoBehaviour, IStart<T0, T1>
        {
            T go = gameObject.AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T StartComponent<T, T0, T1, T2>(this GameObject gameObject, T0 arg0, T1 arg1, T2 arg2) where T : MonoBehaviour, IStart<T0, T1, T2>
        {
            T go = gameObject.AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T StartComponent<T, T0, T1, T2, T3>(this GameObject gameObject, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : MonoBehaviour, IStart<T0, T1, T2, T3>
        {
            T go = gameObject.AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3));
            return go;
        }

        /// <summary> Creates a new gameObject with specified component and calls Init method on it </summary>
        /// <returns> Created (& initialized) gameObject </returns>
        public static T StartComponent<T, T0, T1, T2, T3, T4>(this GameObject gameObject, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : MonoBehaviour, IStart<T0, T1, T2, T3, T4>
        {
            T go = gameObject.AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3, arg4));
            return go;
        }
        #endregion

        #region Extensions
        // @formatter:off
        public static T InitNew<T, T0>                 (this T prefab, T0 arg0) where T : Component, IInit<T0>                                                          => Init(prefab, arg0); 
        public static T InitNew<T, T0, T1>             (this T prefab, T0 arg0, T1 arg1) where T : Component, IInit<T0, T1>                                             => Init(prefab, arg0, arg1); 
        public static T InitNew<T, T0, T1, T2>         (this T prefab, T0 arg0, T1 arg1, T2 arg2) where T : Component, IInit<T0, T1, T2>                                => Init(prefab, arg0, arg1, arg2); 
        public static T InitNew<T, T0, T1, T2, T3>     (this T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : Component, IInit<T0, T1, T2, T3>                   => Init(prefab, arg0, arg1, arg2, arg3); 
        public static T InitNew<T, T0, T1, T2, T3, T4> (this T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : Component, IInit<T0, T1, T2, T3, T4>      => Init(prefab, arg0, arg1, arg2, arg3, arg4); 
        public static T StartNew<T, T0>                (this T prefab, T0 arg0) where T : MonoBehaviour, IStart<T0>                                                     => Start(prefab, arg0); 
        public static T StartNew<T, T0, T1>            (this T prefab, T0 arg0, T1 arg1) where T : MonoBehaviour, IStart<T0, T1>                                        => Start(prefab, arg0, arg1); 
        public static T StartNew<T, T0, T1, T2>        (this T prefab, T0 arg0, T1 arg1, T2 arg2) where T : MonoBehaviour, IStart<T0, T1, T2>                           => Start(prefab, arg0, arg1, arg2); 
        public static T StartNew<T, T0, T1, T2, T3>    (this T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3) where T : MonoBehaviour, IStart<T0, T1, T2, T3>              => Start(prefab, arg0, arg1, arg2, arg3); 
        public static T StartNew<T, T0, T1, T2, T3, T4>(this T prefab, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : MonoBehaviour, IStart<T0, T1, T2, T3, T4> => Start(prefab, arg0, arg1, arg2, arg3, arg4); 
        // @formatter:on
        #endregion

    }

    /// <summary> Interface for Components that need initialization. Call (PREFAB with this component).InitNew or (Any MonoBehaviour).InitObject </summary>
    public interface IInit<T0>
    {
        public void Init(T0 arg0);
    }

    /// <summary> Interface for Components that need initialization. Call (PREFAB with this component).InitNew or (Any MonoBehaviour).InitObject </summary>
    public interface IInit<T0, T1>
    {
        public void Init(T0 arg0, T1 arg1);
    }

    /// <summary> Interface for Components that need initialization. Call (PREFAB with this component).InitNew or (Any MonoBehaviour).InitObject </summary>
    public interface IInit<T0, T1, T2>
    {
        public void Init(T0 arg0, T1 arg1, T2 arg2);
    }

    /// <summary> Interface for Components that need initialization. Call (PREFAB with this component).InitNew or (Any MonoBehaviour).InitObject </summary>
    public interface IInit<T0, T1, T2, T3>
    {
        public void Init(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }

    /// <summary> Interface for Components that need initialization. Call (PREFAB with this component).InitNew or (Any MonoBehaviour).InitObject </summary>
    public interface IInit<T0, T1, T2, T3, T4>
    {
        public void Init(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }

    /// <summary> Interface for Components that need to call a coroutine to initialized. Call (PREFAB with this component).StartNew or (Any MonoBehaviour).StartObject </summary>
    public interface IStart<T0>
    {
        public IEnumerator OnStart(T0 arg0);
    }

    /// <summary> Interface for Components that need to call a coroutine to initialized. Call (PREFAB with this component).StartNew or (Any MonoBehaviour).StartObject </summary>
    public interface IStart<T0, T1>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1);
    }

    /// <summary> Interface for Components that need to call a coroutine to initialized. Call (PREFAB with this component).StartNew or (Any MonoBehaviour).StartObject </summary>
    public interface IStart<T0, T1, T2>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1, T2 arg2);
    }

    /// <summary> Interface for Components that need to call a coroutine to initialized. Call (PREFAB with this component).StartNew or (Any MonoBehaviour).StartObject </summary>
    public interface IStart<T0, T1, T2, T3>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }

    /// <summary> Interface for Components that need to call a coroutine to initialized. Call (PREFAB with this component).StartNew or (Any MonoBehaviour).StartObject </summary>
    public interface IStart<T0, T1, T2, T3, T4>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}
using System;
using UnityEngine;

namespace Omnix.DesignPatterns
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        [Obsolete("Use Init Instead", true)]
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Awake but only called on the actual instance
        /// </summary>
        protected virtual void Init()
        {
            
        }
    }
    
    public abstract class SingletonNeverNull<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new GameObject($"[{typeof(T).Name}]").AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
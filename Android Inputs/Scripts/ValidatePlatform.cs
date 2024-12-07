using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace R.PlatformHandling
{
    public class ValidatePlatform : MonoBehaviour
    {
        [Serializable]
        public class Paired
        {
            public GameObject mobile;
            public GameObject pc;
        }

        [Serializable]
        public class PairedPrefab
        {
            public Transform parent;
            public GameObject mobile;
            public GameObject pc;
        }

        [SerializeField, FormerlySerializedAs("_mobile")] private GameObject[] _activeOnMobile;
        [SerializeField, FormerlySerializedAs("_pc")] private GameObject[] _activeOnPc;
        [SerializeField] private PairedPrefab[] _prefabs;
        [SerializeField] private Paired[] _pairs;

#if UNITY_EDITOR
        [Space]
        [Header("Editor Only")]
        [SerializeField] private Platform _testingPlatform;
#endif

        private void Awake()
        {
            {
                Choose(_activeOnMobile, _activeOnPc, out GameObject[] activate, out GameObject[] destroy);

                foreach (GameObject go in activate)
                {
                    if (go != null)
                        go.SetActive(true);
                }

                foreach (GameObject go in destroy)
                {
                    DestroyObject(go);
                }
            }


            foreach (PairedPrefab pair in _prefabs)
            {
                Choose(pair.mobile, pair.pc, out GameObject prefab, out var _);
                Instantiate(prefab, pair.parent);
            }

            foreach (Paired pair in _pairs)
            {
                Choose(pair.mobile, pair.pc, out GameObject activate, out GameObject deactivate);
                if (activate != null)
                    activate.SetActive(true);
                DestroyObject(deactivate);
            }
        }

        private void Choose<T>(T mobile, T pc, out T keep, out T remove)
        {
#if UNITY_EDITOR
            if (_testingPlatform == Platform.Mobile)
            {
                keep = mobile;
                remove = pc;
            }
            else
            {
                keep = pc;
                remove = mobile;
            }
#elif UNITY_STANDALONE
            keep = pc;
            remove = mobile;
#elif UNITY_ANDROID || UNITY_IOS
            keep = mobile;
            remove = pc;
#endif
        }

        private static void DestroyObject(GameObject target)
        {
            if (target == null) return;

#if UNITY_EDITOR
            target.SetActive(false);
#else
            GameObject.Destroy(target);
#endif
        }
    }
}
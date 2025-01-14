using UnityEngine;

namespace InteractionSystem
{
    [RequireComponent(typeof(Camera))]
    public class InteractionCamera : MonoBehaviour
    {
        public static Camera Instance { get; private set; }
        public static Transform Transform { get; private set; }
#if UNITY_EDITOR
        private static bool HasMultipleInstances = false;
#endif

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = GetComponent<Camera>();
                Transform = transform;
            }
            else
            {
                #if UNITY_EDITOR
                HasMultipleInstances = true;
                #endif
                Debug.LogError($"There are multiple instances of {nameof(InteractionCamera)} in this scene. Please Ensure there is exactly one instance.");
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (HasMultipleInstances)
                Debug.LogError($"There are multiple instances of {nameof(InteractionCamera)} in this scene. Please Ensure there is exactly one instance.");
        }
#endif
    }
}
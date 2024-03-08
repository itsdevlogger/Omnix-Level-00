using UnityEngine;

namespace InteractionSystem
{
    [RequireComponent(typeof(Camera))]
    public class InteractionCamera : MonoBehaviour
    {
        public static Camera Instance { get; private set; }

        private void Awake()
        {
            Instance = GetComponent<Camera>();
        }
    }
}
using UnityEngine;

namespace DebugToScreen
{
    public class LogAtStart : MonoBehaviour
    {
        [SerializeField] private string text;
        [SerializeField] private int priority = 1;
        [SerializeField, Tooltip("Duration -1 meaning this message will be there forever")] 
        private float duration = -1f;

        void Start()
        {
            if (duration <= 0) GameDebug.Log(text, priority);
            else GameDebug.LogTemp(text, duration, priority);
        }
    }

}
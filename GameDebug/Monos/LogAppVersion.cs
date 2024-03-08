using UnityEngine;

namespace DebugToScreen
{
    public class LogAppVersion : MonoBehaviour
    {
        [SerializeField] private string prefix;

        void Start()
        {
            GameDebug.Log($"{prefix}{Application.version}", -1);
        }
    }
}
using UnityEngine;

public class PlatformSpecific : MonoBehaviour
{
    private enum Platform
    {
        Mobile,
        PC
    }

    [SerializeField] private Platform _platform;

#if UNITY_EDITOR
    [SerializeField] private Platform _testingPlatform;
#endif

    private void Awake()
    {
        // Check platform-specific logic
        if (IsPlatformMismatch())
        {
            Debug.Log($"Destroying {gameObject.name} because it doesn't match the target platform ({_platform})");
#if UNITY_EDITOR
            gameObject.SetActive(_platform == _testingPlatform);
#else
            Destroy(gameObject);
#endif
        }

    }

    private bool IsPlatformMismatch()
    {
#if UNITY_EDITOR
        return _platform != _testingPlatform;
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        return _platform != Platform.PC;
#elif UNITY_ANDROID || UNITY_IOS
        return _platform != Platform.Mobile;
#else
        return true; // Default to mismatch for unsupported platforms
#endif
    }
}

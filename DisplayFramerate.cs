using UnityEngine;

public class DisplayFramerate : MonoBehaviour
{
    private string _fpsText;
    private string _versionText;
    private float _deltaTime;
    private int _framesCount;
    private GUIStyle _style;
    private int _lastKnownScreenHeight;

    private void Awake()
    {
        _deltaTime = 0.0f;
        _fpsText = $"FPS: 0";
        _versionText = $"V: {Application.version}";
    }

    void Update()
    {
        _deltaTime += Time.unscaledDeltaTime;
        _framesCount++;

        if (_deltaTime > 0.1f)
        {
            _fpsText = $"FPS: {_framesCount * 10}";
            _framesCount = 0;
            _deltaTime = 0f;
        }
    }

    void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (_style == null || height != _lastKnownScreenHeight) { 
            _style = new GUIStyle();
            _style.alignment = TextAnchor.LowerLeft;
            _style.fontSize = height * 2 / 100;
            _style.normal.textColor = Color.white;
            _lastKnownScreenHeight = height;
        }

        Rect rect = new Rect(10, height * 0.94f, width, height * 0.02f);
        GUI.Label(rect, _fpsText, _style);
        rect.y += rect.height;
        GUI.Label(rect, _versionText, _style);
    }
}

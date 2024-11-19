using UnityEngine;

public class DisplayFramerate : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate the time between frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Set up the style for the text
        GUIStyle style = new GUIStyle();
        int width = Screen.width, height = Screen.height;

        Rect rect = new Rect(10, 10, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100; // Font size (2% of screen height)
        style.normal.textColor = Color.white;

        // Calculate FPS and format the text
        float fps = 1.0f / deltaTime;
        string text = $"FPS: {fps:0.}";

        // Draw the text on the screen
        GUI.Label(rect, text, style);
    }
}

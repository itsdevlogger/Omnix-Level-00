using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Omnix.Damping.Editor
{
    [CustomPropertyDrawer(typeof(BaseDamper<,>), true)]
    public class BaseDamperDrawer : PropertyDrawer
    {
        private static readonly GUIContent FREQUENCY_CONTENT = new GUIContent("Frequency",
            "Frequency of the damper (Hz). Higher values make the movement more responsive and quicker, while lower values make it slower and smoother.");

        private static readonly GUIContent DAMPING_CONTENT = new GUIContent("Damping",
            "Controls how oscillatory the response is. 1.0 = critically damped (no overshoot), <1 = underdamped (bouncy), >1 = overdamped (sluggish).");

        private static readonly GUIContent RESPONSE_CONTENT = new GUIContent("Initial Response",
            "An additional correction factor that affects how the target's velocity influences smoothing. Typically 0 for normal damping");


        private const int GRAPH_HEIGHT = 100;
        private const int GRAPH_RESOLUTION = 200;

        private FieldInfo _isDirtyField;
        private Rect _lastGraphRect;
        private float[] _samples;
        private Transformer _transformer;

        private Vector3[] _points;
        private Vector3[] _pointsAxis1;
        private Vector3[] _pointsAxis0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw foldout
            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);
            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;
            SerializedProperty frequency = property.FindPropertyRelative("_frequency");
            SerializedProperty damping = property.FindPropertyRelative("_damping");
            SerializedProperty initialResponse = property.FindPropertyRelative("_initialResponse");

            EditorGUI.BeginChangeCheck();

            float yOffset = EditorGUIUtility.singleLineHeight + 2;
            var propRect = new Rect(position.x, position.y + yOffset, position.width,
                EditorGUIUtility.singleLineHeight);
            frequency.floatValue = EditorGUI.Slider(propRect, FREQUENCY_CONTENT, frequency.floatValue, 0.1f, 10f);

            propRect.y += yOffset;
            damping.floatValue = EditorGUI.Slider(propRect, DAMPING_CONTENT, damping.floatValue, 0f, 2f);

            propRect.y += yOffset;
            initialResponse.floatValue =
                EditorGUI.Slider(propRect, RESPONSE_CONTENT, initialResponse.floatValue, -4f, 4f);

            bool changed = EditorGUI.EndChangeCheck();

            if (changed || _samples == null)
                RecalculateSamples(frequency.floatValue, damping.floatValue, initialResponse.floatValue);


            Rect graphRect = new Rect(position.x + 17f, position.y + 5f + yOffset * 4f, position.width - 17f,
                GRAPH_HEIGHT);
            if (changed || _points == null || graphRect != _lastGraphRect) RecalculatePoints(graphRect);
            DrawGraph(graphRect);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight * 4f + GRAPH_HEIGHT + 15f;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        private void RecalculateSamples(float frequency, float damping, float initialResponse)
        {
            _samples = new float[GRAPH_RESOLUTION];
            float min = float.MaxValue;
            float max = float.MinValue;

            var damper = new FloatDamper(frequency, damping, initialResponse);

            float dt = 1f / (float)(GRAPH_RESOLUTION - 1);
            damper.Compute(0f, dt);
            for (int i = 0; i < GRAPH_RESOLUTION; i++)
            {
                // float t = i / (float)(RESOLUTION - 1) * 2.0f; // Normalized time from 0 to 2 sec
                float value = damper.Compute(1f, dt); //SimulateDamperResponse(t, frequency, damping, initialResponse);
                if (value < min) min = value;
                if (value > max) max = value;
                _samples[i] = value;
            }

            if (1f > max) max = 1f;

            _transformer = new Transformer(min, max);
            for (int i = 0; i < GRAPH_RESOLUTION; i++)
                _samples[i] = _transformer.Transform(_samples[i]);
        }

        private void RecalculatePoints(Rect rect)
        {
            _lastGraphRect = rect;
            _points = new Vector3[GRAPH_RESOLUTION];
            for (int i = 0; i < GRAPH_RESOLUTION; i++)
            {
                float response = _samples[i];
                float x = rect.x + i * (rect.width / (GRAPH_RESOLUTION - 1));
                float y = rect.y + rect.height - response * rect.height;
                _points[i] = new Vector3(x, y);
            }

            {
                _pointsAxis0 = new Vector3[2];
                var zeroTransformed = _transformer.Transform(0);
                float py = rect.y + rect.height - zeroTransformed * rect.height;
                var point = new Vector3(rect.x, py);
                _pointsAxis0[0] = point;
                point.x += rect.width;
                _pointsAxis0[1] = point;
            }

            {
                _pointsAxis1 = new Vector3[2];
                float py = rect.y + rect.height - _transformer.Transform(1) * rect.height;
                var point = new Vector3(rect.x, py);
                _pointsAxis1[0] = point;
                point.x += rect.width;
                _pointsAxis1[1] = point;
            }
        }

        private void DrawGraph(Rect rect)
        {
            Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0, 0, 0.1f), Color.gray);
            Handles.color = Color.cyan;
            Handles.DrawAAPolyLine(2f, _points);

            Handles.color = Color.white;
            Handles.DrawAAPolyLine(2f, _pointsAxis0);

            Handles.color = Color.yellow;
            Handles.DrawAAPolyLine(2f, _pointsAxis1);

            Rect zeroLabelRect = new Rect(_pointsAxis0[0].x - 15, _pointsAxis0[0].y - 10, 20, 20);
            GUI.Label(zeroLabelRect, "0");

            Rect oneLabelRect = new Rect(_pointsAxis1[0].x - 15, _pointsAxis1[0].y - 10, 20, 20);
            GUI.Label(oneLabelRect, "1");
        }
    }
}
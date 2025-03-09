using UnityEditor;
using UnityEngine;
using Omnix.Damping.Utils;

namespace Omnix.Damping.Editor
{
    [CustomEditor(typeof(BaseSmoothFollower), true)]
    [CanEditMultipleObjects]
    public class SmoothFollowEditor : UnityEditor.Editor
    {
        private SerializedProperty _sourcesProperty;
        private SerializedProperty _followXProperty;
        private SerializedProperty _followYProperty;
        private SerializedProperty _followZProperty;
        private SerializedProperty _skipFramesProperty;
        private SerializedProperty _damperProperty;

        protected virtual void OnEnable()
        {
            _sourcesProperty = serializedObject.FindProperty("_sources");
            _followXProperty = serializedObject.FindProperty("_followX");
            _followYProperty = serializedObject.FindProperty("_followY");
            _followZProperty = serializedObject.FindProperty("_followZ");
            _skipFramesProperty = serializedObject.FindProperty("_skipFrames");
            _damperProperty = serializedObject.FindProperty("_damper");
        }

        public sealed override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawInspector();
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInspector()
        {
            DrawFollowSettings(EditorGUILayout.GetControlRect());
            EditorGUILayout.PropertyField(_skipFramesProperty);
            EditorGUILayout.PropertyField(_damperProperty);
            EditorGUILayout.PropertyField(_sourcesProperty);
        }

        private void DrawFollowSettings(Rect rect)
        {
            float totalWidth = rect.width;
            rect.width = EditorGUIUtility.labelWidth;
            GUI.Label(rect, "Follow Axis");

            rect.x += rect.width;
            rect.width = (totalWidth - rect.width) / 3f;
            rect.width -= 10f;
            _followXProperty.boolValue = EditorGUI.ToggleLeft(rect, "X", _followXProperty.boolValue);
            
            rect.x += rect.width + 5f;
            _followYProperty.boolValue = EditorGUI.ToggleLeft(rect, "Y", _followYProperty.boolValue);

            rect.x += rect.width + 5f;
            _followZProperty.boolValue = EditorGUI.ToggleLeft(rect, "Z", _followZProperty.boolValue);
        }
    }
}
using Omnix.Damping.Utils;
using UnityEditor;
using UnityEngine;

namespace Omnix.Damping.Editor
{
    [CustomEditor(typeof(SmoothLookAt), true)]
    public class SmoothLookAtEditor :  SmoothFollowEditor
    {
        private SerializedProperty _useWorldUpProperty;
        private SerializedProperty _rollProperty;
        private SerializedProperty _worldUpObjectProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _useWorldUpProperty = serializedObject.FindProperty("_useWorldUp");
            _rollProperty = serializedObject.FindProperty("_roll");
            _worldUpObjectProperty = serializedObject.FindProperty("_worldUpObject");
        }


        protected override void DrawInspector()
        {
            base.DrawInspector();
            EditorGUILayout.PropertyField(_useWorldUpProperty);
            
            GUI.enabled = !_useWorldUpProperty.boolValue;
            EditorGUILayout.PropertyField(_rollProperty);
            
            GUI.enabled = !GUI.enabled;
            EditorGUILayout.PropertyField(_worldUpObjectProperty);
            
            GUI.enabled = true;
        }
    }
}
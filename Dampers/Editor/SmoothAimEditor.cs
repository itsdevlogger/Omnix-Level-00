using Omnix.Damping.Utils;
using UnityEditor;
using UnityEngine;

namespace Omnix.Damping.Editor
{
    [CustomEditor(typeof(SmoothAim), true)]
    public class SmoothAimEditor :  SmoothFollowEditor
    {
        private SerializedProperty _aimVectorProperty;
        private SerializedProperty _upVectorProperty;
        private SerializedProperty _worldUpTypeProperty;
        private SerializedProperty _worldUpObjectProperty;
        private SerializedProperty _worldUpVectorProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _aimVectorProperty = serializedObject.FindProperty("_aimVector");
            _upVectorProperty = serializedObject.FindProperty("_upVector");
            _worldUpTypeProperty = serializedObject.FindProperty("_worldUpType");
            _worldUpVectorProperty = serializedObject.FindProperty("_worldUpVector");
            _worldUpObjectProperty = serializedObject.FindProperty("_worldUpObject");
        }


        protected override void DrawInspector()
        {
            base.DrawInspector();
            EditorGUILayout.PropertyField(_aimVectorProperty);
            EditorGUILayout.PropertyField(_upVectorProperty);
            EditorGUILayout.PropertyField(_worldUpTypeProperty);
            
            int wut = _worldUpTypeProperty.enumValueIndex;
            
            GUI.enabled = wut is 2 or 3;
            EditorGUILayout.PropertyField(_worldUpVectorProperty);
            
            GUI.enabled = wut is 1 or 2;
            EditorGUILayout.PropertyField(_worldUpObjectProperty);
            
            GUI.enabled = true;
        }
    }
}
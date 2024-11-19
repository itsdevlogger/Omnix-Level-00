using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ComponentInfoAttribute : Attribute
{
    public string Info { get; }

    public ComponentInfoAttribute(string info)
    {
        Info = info;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
public class ComponentInfoEditor : Editor
{
    GUIContent _content;
    float _lines;
    bool _hasInfo;

    private void OnEnable()
    {
        var component = target.GetType();
        var attribute = (ComponentInfoAttribute)Attribute.GetCustomAttribute(component, typeof(ComponentInfoAttribute));
        _hasInfo = attribute != null;
        if (_hasInfo)
        {
            _content = new GUIContent(attribute.Info);
            _lines = attribute.Info.Split('\n').Length;
        }
    }

    public override void OnInspectorGUI()
    {
        if (_hasInfo)
        {
            EditorGUILayout.Space();
            var color = GUI.color;
            GUI.color = new Color(0.59765625f, 0.80859375f, 0.71875f);
            EditorGUILayout.LabelField(_content, EditorStyles.boldLabel, GUILayout.Height(EditorGUIUtility.singleLineHeight * _lines));
            GUI.color = color;
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight / 2f);
        }
        DrawDefaultInspector();
    }
}
#endif
using InteractionSystem.Interactables;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IrnDoor))]
[CanEditMultipleObjects]
public class IrnDoorEditor : Editor
{
    private SerializedProperty _targetProp;
    private SerializedProperty _openPositionProp;
    private SerializedProperty _closePositionProp;
    private SerializedProperty _animationCurveProp;
    private SerializedProperty _animationDurationProp;
    private SerializedProperty _copyPositionProp;
    private SerializedProperty _copyRotationProp;
    private SerializedProperty _autocloseDelayProp;
    private SerializedProperty _isOpenProp;
    private SerializedProperty _allowIntercept;
    private SerializedProperty _interceptionCurve;


    private void OnEnable()
    {
        _targetProp = serializedObject.FindProperty("target");
        _openPositionProp = serializedObject.FindProperty("openPosition");
        _closePositionProp = serializedObject.FindProperty("closePosition");

        _animationCurveProp = serializedObject.FindProperty("animationCurve");
        _animationDurationProp = serializedObject.FindProperty("animationDuration");
        _copyPositionProp = serializedObject.FindProperty("copyPosition");
        _copyRotationProp = serializedObject.FindProperty("copyRotation");
        
        _autocloseDelayProp = serializedObject.FindProperty("_autocloseDelay");
        _isOpenProp = serializedObject.FindProperty("_isOpen");
        _allowIntercept = serializedObject.FindProperty("allowIntercept");
        _interceptionCurve = serializedObject.FindProperty("interceptionCurve");
    }


    public override void OnInspectorGUI()
    {
        var door = (IrnDoor)target;
        EditorGUI.BeginChangeCheck();
        _targetProp.isExpanded = EditorGUILayout.Foldout(_targetProp.isExpanded, "Referances");
        if (_targetProp.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_targetProp);
            EditorGUILayout.PropertyField(_openPositionProp);
            EditorGUILayout.PropertyField(_closePositionProp);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        _animationCurveProp.isExpanded = EditorGUILayout.Foldout(_animationCurveProp.isExpanded, "Animation");
        if (_animationCurveProp.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_animationCurveProp);
            EditorGUILayout.PropertyField(_animationDurationProp);

            Rect rect = EditorGUILayout.GetControlRect(true);
            float totalWidth = rect.width;
            rect.width = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(rect, "Copy");

            rect.x = rect.width + rect.x * 0.4f;
            rect.width = (totalWidth - rect.width) / 2f;
            _copyPositionProp.boolValue = EditorGUI.ToggleLeft(rect, "Position", _copyPositionProp.boolValue);
            rect.x += rect.width;
            _copyRotationProp.boolValue = EditorGUI.ToggleLeft(rect, "Rotation", _copyRotationProp.boolValue);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        _autocloseDelayProp.isExpanded = EditorGUILayout.Foldout(_autocloseDelayProp.isExpanded, "Settings");
        if (_autocloseDelayProp.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_isOpenProp);
            bool isAutoclose = door.IsAutoclose;
            bool autoClose = EditorGUILayout.Toggle("Auto Close", isAutoclose);
            if (autoClose)
            {
                EditorGUI.indentLevel++;
                if (_autocloseDelayProp.floatValue < 0f) _autocloseDelayProp.floatValue = 0f;
                EditorGUILayout.PropertyField(_autocloseDelayProp);
                if (_autocloseDelayProp.floatValue < 0) _autocloseDelayProp.floatValue = 0f;
                EditorGUI.indentLevel--;
            }
            else if (_autocloseDelayProp.floatValue >= 0f)
            {
                _autocloseDelayProp.floatValue = -1f;
            }
            EditorGUILayout.PropertyField(_allowIntercept);
            if (_allowIntercept.boolValue) 
                EditorGUILayout.PropertyField(_interceptionCurve); 
            EditorGUI.indentLevel--;
        }

        bool hasTarget = door.target != null;
        bool hasOpenPos = door.openPosition != null;
        bool hasClosePos = door.closePosition != null;
        bool guiEnabled = GUI.enabled;
        bool hasChanged = EditorGUI.EndChangeCheck();
        
        EditorGUILayout.Space();
        var height = GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = hasTarget && hasOpenPos;
        if (GUILayout.Button("Set Open", height))
        {
            Undo.RecordObject(door.target, "Set Open");
            if (door.copyPosition) door.target.localPosition = door.openPosition.localPosition;
            if (door.copyRotation) door.target.localRotation = door.openPosition.localRotation;
            EditorUtility.SetDirty(door.target);
            hasChanged = true;
        }

        GUI.enabled = hasTarget && hasClosePos;
        if (GUILayout.Button("Set Close", height))
        {
            Undo.RecordObject(door.target, "Set Close");
            if (door.copyPosition) door.target.localPosition = door.closePosition.localPosition;
            if (door.copyRotation) door.target.localRotation = door.closePosition.localRotation;
            EditorUtility.SetDirty(door.target);
            hasChanged = true;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUI.enabled = hasTarget && hasOpenPos;
        if (GUILayout.Button("Copy to Open", height))
        {
            Undo.RecordObject(door.openPosition, "Copied position to Opened");
            door.openPosition.localPosition = door.target.localPosition;
            door.openPosition.localRotation = door.target.localRotation;
            EditorUtility.SetDirty(door.openPosition);
            hasChanged = true;
        }

        GUI.enabled = hasTarget && hasClosePos;
        if (GUILayout.Button("Copy to Close", height))
        {
            Undo.RecordObject(door.closePosition, "Copied position to Closed");
            door.closePosition.localPosition = door.target.localPosition;
            door.closePosition.localRotation = door.target.localRotation;
            EditorUtility.SetDirty(door.closePosition);
            hasChanged = true;
        }
        EditorGUILayout.EndHorizontal();
        
        if (hasChanged)
        {
            serializedObject.ApplyModifiedProperties();
        }
        
        GUI.enabled = guiEnabled;
    }
}

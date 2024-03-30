using System.Collections.Generic;
using JetBrains.Annotations;
using MenuManagement.Base;
using UnityEditor;
using UnityEngine;

namespace MenuManagement.Editor
{
    public class BasePropertyGroupDrawer : List<SerializedProperty>
    {
        private const string ALL_SET_MESSAGE = "";

        public static class ActiveColors
        {
            public static readonly Color GOOD_BACKGROUND = new Color(1f, 1f, 1f, 0.75f);
            public static readonly Color GOOD_CONTENT = Color.white;
            public static readonly Color BAD_BACKGROUND = new Color(1f, 0f, 0f, 0.75f);
            public static readonly Color BAD_CONTENT = new Color(1f, 0.6f, 0.64f);
        }

        public static class InactiveColors
        {
            public static readonly Color GOOD_BACKGROUND = new Color(1f, 1f, 1f, 0.25f);
            public static readonly Color GOOD_CONTENT = new Color(1f, 1f, 1f, 0.75f);
            public static readonly Color BAD_BACKGROUND = new Color(1f, 0f, 0f, 0.75f);
            public static readonly Color BAD_CONTENT = new Color(1f, 0.6f, 0.64f, 0.76f);
        }

        private readonly GUIContent _headingContent;
        public bool isExpanded;
        public string tooltip;

        public string Title
        {
            get => _headingContent.text;
            set => _headingContent.text = value;
        }

        public bool IsEverythingGood { get; private set; }

        public BasePropertyGroupDrawer(string titleAndTooltip)
        {
            _.SplitName(titleAndTooltip, out string myName, out tooltip);
            _headingContent = new GUIContent(myName, ALL_SET_MESSAGE);
            IsEverythingGood = true;
        }

        public BasePropertyGroupDrawer(string title, string tooltip)
        {
            IsEverythingGood = true;
            _headingContent = new GUIContent(title, ALL_SET_MESSAGE);
            this.tooltip = tooltip;
        }

        public void UpdateProps(string[] propNames, Dictionary<string, SerializedProperty> properties)
        {
            foreach (string name in propNames)
            {
                if (properties.TryGetValue(name, out var sp))
                {
                    Add(sp);
                    properties.Remove(name);
                }
            }
        }

        public void Draw()
        {
            isExpanded = DrawHeader();
            if (isExpanded) DrawGroup();
        }

        /// <param name="errorMessage"> if null, then all is good, else the error message will be displayed.</param>
        public void SetErrorMessage([CanBeNull] string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                IsEverythingGood = true;
                _headingContent.tooltip = ALL_SET_MESSAGE;
            }
            else
            {
                IsEverythingGood = false;
                _headingContent.tooltip = errorMessage;
            }
        }

        protected virtual void DrawGroup()
        {
            foreach (SerializedProperty property in this)
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private bool DrawHeader()
        {
            Color backgroundColor = GUI.backgroundColor;
            Color contentColor = GUI.contentColor;
            switch (isExpanded)
            {
                case true when IsEverythingGood:
                    GUI.backgroundColor = ActiveColors.GOOD_BACKGROUND;
                    GUI.contentColor = ActiveColors.GOOD_CONTENT;
                    break;
                case true when !IsEverythingGood:
                    GUI.backgroundColor = ActiveColors.BAD_BACKGROUND;
                    GUI.contentColor = ActiveColors.BAD_CONTENT;
                    break;
                case false when IsEverythingGood:
                    GUI.backgroundColor = InactiveColors.GOOD_BACKGROUND;
                    GUI.contentColor = InactiveColors.GOOD_CONTENT;
                    break;
                case false when !IsEverythingGood:
                    GUI.backgroundColor = InactiveColors.BAD_BACKGROUND;
                    GUI.contentColor = InactiveColors.BAD_CONTENT;
                    break;
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight * 0.25f);
            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 1.5f);
            rect.width += 10f;
            rect.x -= 10f;

            if (GUI.Button(rect, _headingContent))
            {
                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = contentColor;
                return !isExpanded;
            }

            if (isExpanded && string.IsNullOrEmpty(tooltip) == false)
            {
                rect = EditorGUILayout.GetControlRect(true, 20f);
                rect.x += 5f;
                GUI.contentColor = new Color(0.58f, 1f, 0.98f, 0.51f);
                EditorGUI.LabelField(rect, tooltip);
            }

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;
            return isExpanded;
        }
    }
}
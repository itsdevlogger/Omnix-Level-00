using System.Collections.Generic;
using JetBrains.Annotations;
using MenuManagement.Base;
using UnityEditor;
using UnityEngine;

namespace MenuManagement.Editor
{
    public class BasePropertyGroupDrawer : List<SerializedProperty>
    {
        private const string AllSetMessage = "";
        private static readonly Color HeaderActiveGoodColor = new Color(1f, 1f, 1f, 0.75f);
        private static readonly Color HeaderActiveBadColor = new Color(1f, 0f, 0f, 0.75f);
        private static readonly Color HeaderInactiveGoodColor = new Color(1f, 1f, 1f, 0.25f);
        private static readonly Color HeaderInactiveBadColor = new Color(1f, 0f, 0f, 0.25f);
        
        private readonly GUIContent headingContent;
        public bool isExpanded;
        public string Tooltip;
        public string Title
        {
            get => headingContent.text;
            set => headingContent.text = value;
        }
        public bool IsEverythingGood { get; private set; }

        public BasePropertyGroupDrawer(string titleAndTooltip)
        {
            _.SplitName(titleAndTooltip, out string myName, out Tooltip);
            headingContent = new GUIContent(myName, AllSetMessage);
            IsEverythingGood = true;
        }
        
        public BasePropertyGroupDrawer(string title, string tooltip)
        {
            IsEverythingGood = true;
            headingContent = new GUIContent(title, AllSetMessage);
            Tooltip = tooltip;
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
                headingContent.tooltip = AllSetMessage;
            }
            else
            {
                IsEverythingGood = false;
                headingContent.tooltip = errorMessage;
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
            if (isExpanded)
            {
                GUI.color = IsEverythingGood ? HeaderActiveGoodColor : HeaderActiveBadColor;    
            }
            else
            {
                GUI.color = IsEverythingGood ? HeaderInactiveGoodColor : HeaderInactiveBadColor;    
            }
            
            EditorGUILayout.Space();
            Rect rect = EditorGUILayout.GetControlRect(true, 20f);
            rect.width += 10f;
            rect.x -= 10f;

            if (GUI.Button(rect, headingContent))
            {
                GUI.color = Color.white;
                return !isExpanded;
            }

            if (isExpanded && string.IsNullOrEmpty(Tooltip) == false)
            {
                rect = EditorGUILayout.GetControlRect(true, 20f);
                rect.x += 5f;
                GUI.color = new Color(0.58f, 1f, 0.98f, 0.51f);
                EditorGUI.LabelField(rect, Tooltip);
            }
            GUI.color = Color.white;
            return isExpanded;
        }
    }
}
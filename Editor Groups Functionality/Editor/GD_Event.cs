using MenuManagement.Base;
using UnityEditor;
using UnityEngine;

namespace MenuManagement.Editor
{
    public class GD_Event : BasePropertyGroupDrawer
    {
        public GD_Event() : base(_.Events)
        {
        }

        protected override void DrawGroup()
        {
            float singleLineHeight = EditorGUIUtility.singleLineHeight * 1.25f;
            Rect currentRect = EditorGUILayout.GetControlRect(false, singleLineHeight);
            currentRect.width *= 0.333f;
            int count = 0;

            GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
            style.fixedHeight = singleLineHeight;
            Color defColor = GUI.color;
            foreach (SerializedProperty property in this)
            {
                if (count % 3 == 0)
                {
                    currentRect = EditorGUILayout.GetControlRect(false, singleLineHeight);
                    currentRect.width *= 0.333f;
                }

                GUI.color = property.isExpanded ? Color.green : defColor;
                if (GUI.Button(currentRect, property.displayName.Substring(2), style))
                {
                    property.isExpanded = !property.isExpanded;
                    if (!property.isExpanded)
                    {
                        property.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize = 0;
                    }
                }

                count++;
                if (count == 12) count = 9;
                currentRect.x += currentRect.width;
            }

            GUI.color = defColor;

            foreach (SerializedProperty property in this)
            {
                if (property.isExpanded)
                    EditorGUILayout.PropertyField(property, true);
            }
        }
    }
}
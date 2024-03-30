using UnityEditor;
using UnityEngine;

namespace Omnix.Utils.EditorUtils
{
    public static class ButtonsRow
    {
        private static Rect current;
        private static Rect original;
        private static int miniButtonCount;
        private static GUIStyle buttonLeftAligned;
        private static GUIStyle buttonCenterAligned;
        private static GUILayoutOption[] headerButtonOptions;

        private static float LineHeight => EditorGUIUtility.singleLineHeight;
        private static float MiniButtonWidth => LineHeight * 2f;
        private static float BigButtonWidth => original.width - (MiniButtonWidth + EditorGUIUtility.standardVerticalSpacing) * miniButtonCount;
        private static int PaddingX => (int)(EditorGUIUtility.standardVerticalSpacing * 3f);
        private static int PaddingY => (int)EditorGUIUtility.standardVerticalSpacing;


        private static GUIStyle ButtonLeftAligned
        {
            get
            {
                if (buttonLeftAligned != null) return buttonLeftAligned;

                buttonLeftAligned = new GUIStyle(EditorStyles.toolbarButton)
                {
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(PaddingX, PaddingX, PaddingY, PaddingY)
                };
                return buttonLeftAligned;
            }
        }

        private static GUIStyle ButtonCenterAligned
        {
            get
            {
                if (buttonCenterAligned != null) return buttonCenterAligned;
                buttonCenterAligned = new GUIStyle(EditorStyles.toolbarButton)
                {
                    alignment = TextAnchor.MiddleCenter,
                    padding = new RectOffset(PaddingX, PaddingX, PaddingY, PaddingY)
                };
                return buttonCenterAligned;
            }
        }

        public static bool HeaderButton(string text)
        {
            if (headerButtonOptions == null)
            {
                headerButtonOptions = new[] { GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.5f), GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f) };
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool isClicked = GUILayout.Button(text, headerButtonOptions);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            return isClicked;
        }

        public static void BeginRow(int numberOfMiniButtons, float spaceBefore = 0f, float spaceAfter = 0f)
        {
            original = EditorGUILayout.GetControlRect(false, LineHeight + PaddingY);
            original.x += spaceBefore;
            original.width -= spaceBefore + spaceAfter;
            current = new Rect(original);
            current.x -= current.width + EditorGUIUtility.standardVerticalSpacing;
            miniButtonCount = numberOfMiniButtons;
        }

        public static bool MiniButton(string text)
        {
            current.x += current.width + EditorGUIUtility.standardVerticalSpacing;
            current.width = MiniButtonWidth;
            return GUI.Button(current, text, ButtonCenterAligned);
        }
       
        public static bool MiniButton(GUIContent content)
        {
            current.x += current.width + EditorGUIUtility.standardVerticalSpacing;
            current.width = MiniButtonWidth;
            return GUI.Button(current, content, ButtonCenterAligned);
        }

        public static Object MiniObjectField(Object obj, System.Type objType, bool allowSceneObjects)
        {
            current.x += current.width + EditorGUIUtility.standardVerticalSpacing;
            current.width = MiniButtonWidth;
            return EditorGUI.ObjectField(current, obj, objType, allowSceneObjects);
        }
        
        public static bool BigButton(string text)
        {
            current.x += current.width + EditorGUIUtility.standardVerticalSpacing;
            current.width = BigButtonWidth;
            return GUI.Button(current, text, ButtonLeftAligned);
        }

        public static bool BigButton(GUIContent content)
        {
            current.x += current.width + EditorGUIUtility.standardVerticalSpacing;
            current.width = BigButtonWidth;
            return GUI.Button(current, content, ButtonLeftAligned);
        }
        
        public static Object BigObjectField(Object obj, System.Type objType, bool allowSceneObjects)
        {
            current.x += current.width + EditorGUIUtility.standardVerticalSpacing;
            current.width = BigButtonWidth;
            return EditorGUI.ObjectField(current, obj, objType, allowSceneObjects);
        }
    }
}
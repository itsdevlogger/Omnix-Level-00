using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Omnix.Editor
{
    public static class OmnixMenu
    {
        public const string MAIN_MENU = "Omnix/";
        public const string OBJECT_MENU = "GameObject/" + MAIN_MENU;
        public const string SELECT_MENU = MAIN_MENU + "Selections/";
        public const string WINDOW_MENU = MAIN_MENU + "Windows/";
        public const string STORAGE_MENU = MAIN_MENU + "Select Storage/";

        private static class _
        {
            public const string REMOVE_MISSING_REFERENCE_SCRIPTS = "Remove Missing Reference Scripts";
            public const string REMOVE_DUPLICATE_COMPONENTS = "Remove Multiple Components of Same Type";
            public const string REMOVE_ALL_COMPONENTS = "Remove All Components";
            public const string SORT_COMPONENTS = "Sort Components";
            public const string REPLACE_TEXT_WITH_TMP = "Replace Text with TextMeshProUgui";
            public const string INVERT_SELECTION = "Invert Selection";
            public const string NORMALIZE_BOX_COLLIDER = "Normalize Box Collider";
        }


        /// <summary> Performs given operation on all selected gameObjects and collapse all that in Single Undo operation </summary>
        /// <remarks> Operation must account for undo </remarks>
        public static void WrapInUndo(string undoName, Action<GameObject> operation)
        {
            if (Selection.gameObjects.Length == 1)
            {
                operation(Selection.gameObjects[0]);
                return;
            }

            WrapInUndo(undoName, Selection.gameObjects, operation);
        }


        /// <summary> Performs given operation on all selected gameObjects and collapse all that in Single Undo operation </summary>
        /// <remarks> Operation must account for undo </remarks>
        public static void WrapInUndo<T>(string undoName, IEnumerable<T> targets, Action<T> operation)
        {
            int undoGroupIndex = Undo.GetCurrentGroup();
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(undoName);
            foreach (T target in targets)
            {
                operation(target);
            }

            Undo.CollapseUndoOperations(undoGroupIndex);
        }


        [MenuItem(OBJECT_MENU + _.REMOVE_MISSING_REFERENCE_SCRIPTS, true)]
        [MenuItem(SELECT_MENU + _.REMOVE_MISSING_REFERENCE_SCRIPTS, true)]
        [MenuItem(OBJECT_MENU + _.REMOVE_DUPLICATE_COMPONENTS, true)]
        [MenuItem(SELECT_MENU + _.REMOVE_DUPLICATE_COMPONENTS, true)]
        [MenuItem(OBJECT_MENU + _.REMOVE_ALL_COMPONENTS, true)]
        [MenuItem(SELECT_MENU + _.REMOVE_ALL_COMPONENTS, true)]
        [MenuItem(OBJECT_MENU + _.REPLACE_TEXT_WITH_TMP, true)]
        [MenuItem(SELECT_MENU + _.REPLACE_TEXT_WITH_TMP, true)]
        [MenuItem(OBJECT_MENU + _.INVERT_SELECTION, true)]
        [MenuItem(SELECT_MENU + _.INVERT_SELECTION, true)]
        [MenuItem(OBJECT_MENU + _.SORT_COMPONENTS, true)]
        [MenuItem(SELECT_MENU + _.SORT_COMPONENTS, true)]
        [MenuItem(OBJECT_MENU + _.NORMALIZE_BOX_COLLIDER, true)]
        [MenuItem(SELECT_MENU + _.NORMALIZE_BOX_COLLIDER, true)]
        public static bool IsAnythingSelected() => Selection.gameObjects.Length > 0;


        [MenuItem(OBJECT_MENU + _.REMOVE_MISSING_REFERENCE_SCRIPTS)]
        [MenuItem(SELECT_MENU + _.REMOVE_MISSING_REFERENCE_SCRIPTS)]
        public static void RemoveMissing()
        {
            bool conti = EditorUtility.DisplayDialog("Continue", "This operation can't be undone. Wish to proceed?", "Yes", "No");
            if (conti) WrapInUndo("Remove Missing Reference Scripts", EditorMenuHelpers.RecursiveRemoveComponentsInChildren);
        }

        [MenuItem(OBJECT_MENU + _.REMOVE_DUPLICATE_COMPONENTS)]
        [MenuItem(SELECT_MENU + _.REMOVE_DUPLICATE_COMPONENTS)]
        public static void RemoveDuplicateComponents()
        {
            WrapInUndo("Remove Duplicate Components", EditorMenuHelpers.RemoveDuplicateComponents);
        }

        [MenuItem(OBJECT_MENU + _.REMOVE_ALL_COMPONENTS)]
        [MenuItem(SELECT_MENU + _.REMOVE_ALL_COMPONENTS)]
        public static void RemoveAllComponents()
        {
            WrapInUndo("Remove All Components", EditorMenuHelpers.RemoveAllComponents);
        }

        [MenuItem(OBJECT_MENU + _.SORT_COMPONENTS)]
        [MenuItem(SELECT_MENU + _.SORT_COMPONENTS)]
        public static void SortComponents()
        {
            WrapInUndo("Sort Components By Name", EditorMenuHelpers.SortComponentsByName);
        }


        [MenuItem(OBJECT_MENU + _.INVERT_SELECTION)]
        [MenuItem(SELECT_MENU + _.INVERT_SELECTION)]
        public static void InvertSelection()
        {
            HashSet<GameObject> oldSelection = new HashSet<GameObject>();

            foreach (GameObject obj in Selection.gameObjects)
            {
                oldSelection.Add(obj);
            }

            int newSelCount = 0;
            foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (!oldSelection.Contains(obj))
                {
                    newSelCount++;
                }
            }

            Object[] newSelection = new Object[newSelCount];
            newSelCount = 0;
            foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (!oldSelection.Contains(obj))
                {
                    newSelection[newSelCount] = obj;
                    newSelCount++;
                }
            }

            Selection.objects = newSelection;
        }

        [MenuItem(OBJECT_MENU + _.REPLACE_TEXT_WITH_TMP, false, 1004)]
        [MenuItem(SELECT_MENU + _.REPLACE_TEXT_WITH_TMP, false, 1004)]
        public static void ReplaceText()
        {
            if (TMP_Settings.defaultFontAsset == null)
            {
                EditorUtility.DisplayDialog("ERROR!", "Assign a default font asset in project settings!", "OK");
                return;
            }

            WrapInUndo("Convert Text to TextMeshPro", EditorMenuHelpers.ConvertAllTextToTextMeshPros);
        }

        [MenuItem(MAIN_MENU + "Clear Player Prefs", false, 1001)]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem(MAIN_MENU + "Clear Selections", false, 1002)]
        public static void ClearSelection()
        {
            Selection.activeGameObject = null;
            Selection.activeObject = null;
            Selection.activeTransform = null;
        }

        [MenuItem(MAIN_MENU + "Main Camera to Scene View", false, 1003)]
        public static void MainCameraToSceneView()
        {
            Transform sceneCam = EditorWindow.GetWindow<SceneView>().camera.transform;
            Camera camera = Camera.main;

            if (camera == null)
            {
                EditorUtility.DisplayDialog("Error!", "Camera.main not found", "Okay");
                return;
            }

            Transform gameCam = camera.transform;
            gameCam.position = sceneCam.position;
            gameCam.rotation = sceneCam.rotation;
            gameCam.localScale = sceneCam.localScale;
        }

        [MenuItem(MAIN_MENU + "Toggle Inspector Lock _F3", false, 1004)] // F3 key. if you want to change hotkey: https://docs.unity3d.com/ScriptReference/MenuItem.html#:~:text=To%20create%20a,TAB%2C%20and%20SPACE.
        public static void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

        [MenuItem(OBJECT_MENU + _.NORMALIZE_BOX_COLLIDER)]
        [MenuItem(SELECT_MENU + _.NORMALIZE_BOX_COLLIDER)]
        public static void NormalizeBoxCollider()
        {
            // Get all selected GameObjects
            GameObject[] selectedObjects = Selection.gameObjects;

            foreach (GameObject obj in selectedObjects)
            {
                // Ensure the GameObject has a BoxCollider
                BoxCollider collider = obj.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    continue;
                }

                // Reset the GameObject's scale and position
                Undo.RecordObject(obj.transform, "Adjust BoxCollider Transform");
                var scale = obj.transform.localScale;
                var pos = obj.transform.localPosition;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;

                // Adjust the collider's size and center
                Undo.RecordObject(collider, "Adjust BoxCollider");
                var size = collider.size;
                collider.size = Vector3.Scale(scale, size);
                collider.center += pos;
            }
        }
    }

    public static class EditorMenuHelpers
    {
        private static bool Requires(Type comp, Type requirement)
        {
            if (!Attribute.IsDefined(comp, typeof(RequireComponent)))
            {
                return false;
            }

            foreach (RequireComponent rc in Attribute.GetCustomAttributes(comp, typeof(RequireComponent)).OfType<RequireComponent>())
            {
                if (rc.m_Type0.IsAssignableFrom(requirement)
                    || (rc.m_Type1 != null && rc.m_Type1.IsAssignableFrom(requirement))
                    || (rc.m_Type2 != null && rc.m_Type2.IsAssignableFrom(requirement)))
                {
                    return true;
                }
            }

            return false;
        }
        
        private static bool CanDestroy(this GameObject targetGo, Type componentToDestroy)
        {
            if (!typeof(Component).IsAssignableFrom(componentToDestroy))
                return true;

            foreach (Component component in targetGo.GetComponents<Component>())
            {
                if (Requires(component.GetType(), componentToDestroy))
                    return false;
            }

            return true;
        }

        
        /// <summary> Remove all missing components from an object and its children </summary>
        /// <remarks> Dont register object changed, doesnt Supports undo functionality </remarks>
        public static void RecursiveRemoveComponentsInChildren(GameObject g)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
            foreach (Transform childT in g.transform)
            {
                RecursiveRemoveComponentsInChildren(childT.gameObject);
            }
        }

        /// <summary> Remove multiple components form an target object </summary>
        /// <remarks> Register object changed, Supports undo functionality </remarks>
        public static bool RemoveComponents(GameObject target, Func<Component, bool> check)
        {
            int undoGroupIndex = Undo.GetCurrentGroup();
            Undo.IncrementCurrentGroup();

            int tries = 0;
            bool wantMoreTries = true;

            while (wantMoreTries)
            {
                wantMoreTries = false;

                foreach (Component component in target.GetComponents<Component>())
                {
                    if (component is Transform) continue;
                    if (!check.Invoke(component)) continue;

                    if (!target.CanDestroy(component.GetType()))
                    {
                        wantMoreTries = true;
                        continue;
                    }

                    Undo.DestroyObjectImmediate(component);
                }

                tries++;

                if (tries > 10)
                {
                    Undo.CollapseUndoOperations(undoGroupIndex);
                    EditorUtility.SetDirty(target);
                    return false;
                }
            }

            Undo.CollapseUndoOperations(undoGroupIndex);
            EditorUtility.SetDirty(target);
            return true;
        }

        /// <summary> Sort all components by name. </summary>
        /// <remarks> Registers object changed, Supports undo functionality </remarks>
        public static void SortComponentsByName(GameObject target)
        {
            Undo.RecordObject(target, "Sort Components");
            List<Component> components = target.GetComponents<Component>().ToList();
            components.RemoveAll(x => x.GetType().ToString() == "UnityEngine.Transform");

            for (int p = 0; p <= components.Count - 2; p++)
            {
                for (int i = 0; i <= components.Count - 2; i++)
                {
                    Component c1 = components[i];
                    Component c2 = components[i + 1];

                    string name1 = c1.GetType().ToString().Split('.').Last();
                    string name2 = c2.GetType().ToString().Split('.').Last();

                    if (String.CompareOrdinal(name1, name2) > 0)
                    {
                        ComponentUtility.MoveComponentUp(c2);
                        components = target.GetComponents<Component>().ToList();
                        components.RemoveAll(x => x.GetType().ToString() == "UnityEngine.Transform");
                    }
                }
            }

            EditorUtility.SetDirty(target);
        }

        /// <summary> Remove all missing components from an object </summary>
        /// <remarks> Registers object changed, Supports undo functionality </remarks>
        public static void RemoveMissingComponents(GameObject target, bool searchChildren)
        {
            Undo.RecordObject(target, "Remove Missing Components");
            if (searchChildren) RecursiveRemoveComponentsInChildren(target);
            else GameObjectUtility.RemoveMonoBehavioursWithMissingScript(target);
            EditorUtility.SetDirty(target);
        }

        /// <summary> Remove all duplicate components (Components of same type) from an object </summary>
        /// <remarks> Registers object changed, Supports undo functionality </remarks>
        public static void RemoveDuplicateComponents(GameObject target)
        {
            Dictionary<Type, int> compCount = new Dictionary<Type, int>();
            foreach (Component component in target.GetComponents<Component>())
            {
                Type ct = component.GetType();
                if (compCount.ContainsKey(ct)) compCount[ct] += 1;
                else compCount.Add(ct, 1);
            }

            RemoveComponents(target, component => compCount[component.GetType()] > 1);
        }

        /// <summary> Remove all components from an object </summary>
        /// <remarks> Registers object changed, Supports undo functionality </remarks>
        public static void RemoveAllComponents(GameObject target)
        {
            RemoveComponents(target, _ => true);
        }

        /// <summary> Convert a Text Component to TextMeshPro Component </summary>
        /// <remarks> Registers object changed, Supports undo functionality </remarks>
        public static void ConvertTextToTextMeshPro(Text uiText)
        {
            if (uiText == null) return;

            int undoGroupIndex = Undo.GetCurrentGroup();
            Undo.IncrementCurrentGroup();

            var textInfo = new TextInfo(uiText); // Get all info about the the text
            Undo.DestroyObjectImmediate(uiText); // Destroy the text (Cause we cant and TextMeshProUGUI & Text on same gameObject)

            // Add TextMeshProUGUI
            TextMeshProUGUI tmp = textInfo.gameObject.AddComponent<TextMeshProUGUI>();
            Undo.RegisterCreatedObjectUndo(tmp, "Replace text with text mesh pro");
            Undo.RecordObject(tmp, "Updated Text Mesh Pro");

            tmp.enabled = textInfo.enabled;
            tmp.fontStyle = textInfo.fontStyles;
            tmp.fontSize = textInfo.fontSize;
            tmp.fontSizeMin = textInfo.fontSizeMin;
            tmp.fontSizeMax = textInfo.fontSizeMax;
            tmp.lineSpacing = textInfo.lineSpacing;
            tmp.richText = textInfo.richText;
            tmp.enableAutoSizing = textInfo.enableAutoSizing;
            tmp.alignment = textInfo.textAlignOption;
            tmp.textWrappingMode = TextWrappingModes.Normal;
            tmp.overflowMode = textInfo.overflowMode;
            tmp.text = textInfo.text;
            tmp.color = textInfo.color;
            tmp.raycastTarget = textInfo.raycastTarget;

            EditorUtility.SetDirty(tmp);
            Undo.CollapseUndoOperations(undoGroupIndex);
        }

        /// <summary> Convert all Text Components of target object to TextMeshPro Component </summary>
        /// <remarks> Registers object changed, Supports undo functionality </remarks>
        public static void ConvertAllTextToTextMeshPros(GameObject target)
        {
            foreach (Text text in target.GetComponents<Text>())
            {
                ConvertTextToTextMeshPro(text);
            }
        }


        private class TextInfo
        {
            public GameObject gameObject;
            public bool enabled;
            public int fontSize;
            public int fontSizeMin;
            public int fontSizeMax;
            public float lineSpacing;
            public bool richText;
            public bool enableAutoSizing;
            public bool enableWordWrapping;
            public TextOverflowModes overflowMode;
            public string text;
            public Color color;
            public bool raycastTarget;
            public FontStyles fontStyles;
            public TextAlignmentOptions textAlignOption;

            public TextInfo(Text uiText)
            {
                gameObject = uiText.gameObject;
                enabled = uiText.enabled;
                fontSize = uiText.fontSize;
                fontSizeMin = uiText.resizeTextMinSize;
                fontSizeMax = uiText.resizeTextMaxSize;
                lineSpacing = uiText.lineSpacing;
                richText = uiText.supportRichText;
                enableAutoSizing = uiText.resizeTextForBestFit;
                enableWordWrapping = HorizontalWrapMode.Wrap == uiText.horizontalOverflow;
                overflowMode = uiText.verticalOverflow == VerticalWrapMode.Truncate ? TextOverflowModes.Truncate : TextOverflowModes.Overflow;
                text = uiText.text;
                color = uiText.color;
                raycastTarget = uiText.raycastTarget;

                fontStyles = uiText.fontStyle switch
                {
                    FontStyle.Normal => FontStyles.Normal,
                    FontStyle.Bold => FontStyles.Bold,
                    FontStyle.Italic => FontStyles.Italic,
                    FontStyle.BoldAndItalic => FontStyles.Bold | FontStyles.Italic,
                    _ => FontStyles.Normal
                };

                textAlignOption = uiText.alignment switch
                {
                    TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
                    TextAnchor.UpperCenter => TextAlignmentOptions.Top,
                    TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
                    TextAnchor.MiddleLeft => TextAlignmentOptions.Left,
                    TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
                    TextAnchor.MiddleRight => TextAlignmentOptions.Right,
                    TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
                    TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
                    TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
                    _ => TextAlignmentOptions.TopLeft
                };
            }
        }
    }
}
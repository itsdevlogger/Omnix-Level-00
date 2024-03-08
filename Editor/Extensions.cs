#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using Object = UnityEngine.Object;

namespace Omnix.Utils.EditorUtils
{
    public static class EditorExtensions
    {
        private static object GetFieldValue(object obj, string fieldName,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, bindings);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            return default(object);
        }

        public static object GetFieldValueWithIndex(this object obj, string fieldName, int index,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, bindings);
            if (field != null)
            {
                object list = field.GetValue(obj);
                if (list.GetType().IsArray)
                {
                    return ((object[])list)[index];
                }
                else if (list is IEnumerable)
                {
                    return ((IList)list)[index];
                }
            }

            return default(object);
        }

        public static bool SetFieldValueWithIndex(this object obj, string fieldName, int index, object value,
            bool includeAllBases = false,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, bindings);
            if (field != null)
            {
                object list = field.GetValue(obj);
                if (list.GetType().IsArray)
                {
                    ((object[])list)[index] = value;
                    return true;
                }
                else if (value is IEnumerable)
                {
                    ((IList)list)[index] = value;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Moves the asset inside target if its a folder, else same folder as target asset.
        /// </summary>
        /// <param name="toMove">Asset to move</param>
        /// <param name="target">Target Directory or Asset</param>
        public static void MoveAssetTo(this Object toMove, Object target)
        {
            FileInfo resourcePath = new FileInfo(AssetDatabase.GetAssetPath(target.GetFirstAsset()));

            FileInfo s = new FileInfo(AssetDatabase.GetAssetPath(toMove));
            FileInfo sM = new FileInfo(s.FullName + ".meta");
            FileInfo t = new FileInfo(resourcePath.DirectoryName + "/" + s.Name);

            if (s.DirectoryName == t.DirectoryName)
            {
                return;
            }

            UnityEditor.FileUtil.MoveFileOrDirectory(s.FullName, t.FullName);
            if (sM.Exists)
            {
                UnityEditor.FileUtil.MoveFileOrDirectory(sM.FullName, t.FullName + ".meta");
            }

            AssetDatabase.Refresh();
        }


        /// <summary>
        /// Returns first asset in resource if its a folder, else resource
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Object GetFirstAsset(this Object resource)
        {
            string path = AssetDatabase.GetAssetPath(resource);
            if (!Directory.Exists(path)) return resource;

            foreach (string subPath in Directory.GetDirectories(path))
            {
                if (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(subPath)))
                    return AssetDatabase.LoadAssetAtPath<Object>(subPath);
            }

            foreach (string subPath in Directory.GetFiles(path))
            {
                if (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(subPath)))
                    return AssetDatabase.LoadAssetAtPath<Object>(subPath);
            }

            return resource;
        }

        public static Texture GetIcon(this Object resource)
        {
            Type type = resource.GetType();

            if (Directory.Exists(AssetDatabase.GetAssetPath(resource))) return EditorGUIUtility.IconContent("Folder Icon").image;

            if (typeof(GameObject).IsAssignableFrom(type)) return PrefabUtility.GetIconForGameObject((GameObject)resource);
            if (typeof(ScriptableObject).IsAssignableFrom(type)) return EditorGUIUtility.IconContent("ScriptableObject Icon").image;

            if (type == typeof(SceneAsset)) return EditorGUIUtility.IconContent("UnityLogo").image;
            if (type == typeof(TextAsset)) return EditorGUIUtility.IconContent("TextAsset Icon").image;
            if (type == typeof(MonoScript)) return EditorGUIUtility.IconContent("cs Script Icon").image;
            if (type == typeof(AnimatorController)) return EditorGUIUtility.IconContent("AnimatorController Icon").image;
            if (type == typeof(LightingDataAsset)) return EditorGUIUtility.IconContent("SceneviewLighting").image;
            if (type == typeof(Cubemap)) return EditorGUIUtility.IconContent("PreMatCube").image;
            if (type == typeof(Shader)) return EditorGUIUtility.IconContent("Shader Icon").image;
            if (type == typeof(Texture2D)) return EditorGUIUtility.IconContent("PreTextureRGB").image;
            return null;
        }

        public static string GetGuid(this Object resource)
        {
            return AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(resource)).ToString();
        }

        public static Type GetProperType(this SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            string path = property.propertyPath.Replace(".Array.data", "");
            string[] fieldStructure = path.Split('.');
            Regex rgx = new Regex(@"\[\d+\]");

            for (int i = 0; i < fieldStructure.Length; i++)
            {
                if (fieldStructure[i].Contains("["))
                {
                    int index = System.Convert.ToInt32(new string(fieldStructure[i].Where(c => char.IsDigit(c))
                        .ToArray()));
                    obj = GetFieldValueWithIndex(obj, rgx.Replace(fieldStructure[i], ""), index);
                }
                else
                {
                    obj = GetFieldValue(obj, fieldStructure[i]);
                }
            }

            return obj.GetType();
        }

        public static SerializedProperty FindPropertyChild(this SerializedProperty property, string childName)
        {
            string parentPath = property.propertyPath;
            SerializedProperty iterator = property.Copy();
            while (iterator.Next(true))
            {
                if (iterator.name == childName && iterator.propertyPath.Contains(parentPath))
                {
                    return iterator;
                }
            }

            return null;
        }

        public static Dictionary<string, SerializedProperty> FindPropertyChildren(this SerializedProperty property, HashSet<string> childrenNames)
        {
            Dictionary<string, SerializedProperty> toReturn = new Dictionary<string, SerializedProperty>();
            int currentCount = 0;
            int finalCount = childrenNames.Count;

            string parentPath = property.propertyPath;
            SerializedProperty iterator = property.Copy();
            while (iterator.Next(true))
            {
                if (childrenNames.Contains(iterator.name) && iterator.propertyPath.Contains(parentPath))
                {
                    toReturn.Add(iterator.name, iterator);
                    currentCount++;
                    if (currentCount >= finalCount) return toReturn;
                }
            }

            foreach (string name in childrenNames)
            {
                if (!toReturn.ContainsKey(name)) toReturn.Add(name, null);
            }

            return toReturn;
        }

        private static FieldInfo GetFieldViaPath(this Type type, string path)
        {
            System.Type parentType = type;
            System.Reflection.FieldInfo fi = type.GetField(path);
            string[] perDot = path.Split('.');
            foreach (string fieldName in perDot)
            {
                fi = parentType.GetField(fieldName);
                if (fi != null)
                    parentType = fi.FieldType;
                else
                    return null;
            }

            if (fi != null)
                return fi;
            else return null;
        }

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

        public static bool CanDestroy(this GameObject targetGo, Type componentToDestroy)
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

        public static string ClassName(this Component component)
        {
            string fullName = component.GetType().ToString();
            int index = fullName.LastIndexOf(".");
            if (index > 0)
            {
                return $"{fullName.Substring(index + 1)} ({fullName.Substring(0, index)})";
            }

            return $"{fullName}";
        }

        public static void CopyComponentTo(this Component sourceComp, GameObject targetObject)
        {
            if (ComponentUtility.CopyComponent(sourceComp))
                ComponentUtility.PasteComponentAsNew(targetObject);
        }
    }
}
#endif
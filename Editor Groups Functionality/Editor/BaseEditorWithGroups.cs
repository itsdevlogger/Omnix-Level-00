using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using MenuManagement.Base;
using UnityEditor;
using UnityEngine;
using Attribute = System.Attribute;

namespace MenuManagement.Editor
{
    public class BaseEditorWithGroups : UnityEditor.Editor
    {
        private static readonly Dictionary<string, bool> Values = new Dictionary<string, bool>();

        private SerializedProperty scriptProperty;
        private List<BasePropertyGroupDrawer> groups;
        private List<(string, MethodInfo)> buttons;
        private Dictionary<string, SerializedProperty> allProps;

        private void GetAllProps()
        {
            allProps = new Dictionary<string, SerializedProperty>();
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);

            while (property.NextVisible(false))
            {
                allProps.Add(property.name, property.Copy());
            }
        }

        public SerializedProperty GrabProperty(string propName)
        {
            SerializedProperty prop = allProps[propName];
            allProps.Remove(propName);
            return prop;
        }


        /// <example>
        /// assume the following classes
        /// ```
        /// [Serializable]
        /// [DataContract]
        /// public class Creature
        /// {
        ///
        /// }
        ///
        /// [AnimalInfo(noOfLegs: 4, hasWigns: false, _type: CreatureTypes.Carnivores)]
        /// [AnimalInheritance("Forest")]
        /// [InheritedExport]
        /// public class WildCreature : Creature
        /// {
        ///
        /// }
        ///
        /// [WildAnimaInfo(family: AnimalFamily.Cats)]
        /// public class Tiger : WildCreature
        /// {
        ///
        /// }
        /// ```
        /// If we pass Tiger as the type, then the enumerable will return following attributes in the same order:
        ///     Serializable
        ///     DataContract
        ///     AnimalInfo
        ///     AnimalInheritance
        ///     InheritedExport
        ///     WildAnimaInfo
        /// </example>
        /// <returns> all custom attributes of target type and its base classes.  </returns>
        private static IEnumerable<Attribute> GetCustomAttributeParentToChild(Type targetType)
        {
            if (targetType == null) yield break;

            List<Type> types = new List<Type>();
            while (targetType != typeof(System.Object))
            {
                if (targetType == null) continue;

                types.Add(targetType);
                targetType = targetType.BaseType;
            }

            types.Reverse();

            foreach (Type type in types)
            {
                foreach (Attribute attribute in type.GetCustomAttributes(false))
                {
                    yield return attribute;
                }
            }
        }


        protected virtual IEnumerable<BasePropertyGroupDrawer> RegisterGroups()
        {
            Dictionary<string, BasePropertyGroupDrawer> normalGroups = new Dictionary<string, BasePropertyGroupDrawer>();

            HashSet<BasePropertyGroupDrawer> alreadyAdded = new HashSet<BasePropertyGroupDrawer>();
            foreach (BasePropertyGroupDrawer drawer in groups)
            {
                normalGroups.Add(drawer.Title, drawer);
                alreadyAdded.Add(drawer);
            }

            var runtimeConstants = new GD_RuntimeConstant();
            var events = new GD_Event();
            var readOnly = new GD_ReadOnly();

            foreach (Attribute attribute in GetCustomAttributeParentToChild(target.GetType()))
            {
                if (attribute is GroupProperties attProp)
                {
                    BasePropertyGroupDrawer group;
                    if (normalGroups.TryGetValue(attProp.groupName, out group))
                    {
                        group.UpdateProps(attProp.properties, allProps);
                    }
                    else
                    {
                        group = new BasePropertyGroupDrawer(attProp.groupName, attProp.tooltip);
                        group.UpdateProps(attProp.properties, allProps);
                        normalGroups.Add(attProp.groupName, group);
                    }
                }
                else if (attribute is GroupRuntimeConstant attRtc)
                {
                    runtimeConstants.UpdateProps(attRtc.properties, allProps);
                }
                else if (attribute is GroupEvents attEvt)
                {
                    events.UpdateProps(attEvt.properties, allProps);
                }
                else if (attribute is GroupReadOnly attReadOnly)
                {
                    readOnly.UpdateProps(attReadOnly.properties, allProps);
                }
            }

            
            var defaults = new BasePropertyGroupDrawer(_.Unsorted);
            defaults.AddRange(allProps.Values);

            if (runtimeConstants.Count > 0) yield return runtimeConstants;
            if (readOnly.Count > 0) yield return readOnly;

            foreach (BasePropertyGroupDrawer drawer in normalGroups.Values)
            {
                if (drawer.Count > 0 && alreadyAdded.Contains(drawer) == false)
                {
                    yield return drawer;
                }
            }

            if (defaults.Count > 0) yield return defaults;
            if (events.Count > 0) yield return events;
        }

        protected virtual void OnEnable()
        {
            groups = new List<BasePropertyGroupDrawer>();
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            scriptProperty = property.Copy();
            GetAllProps();

            string suffix = target.GetType().Name;
            foreach (BasePropertyGroupDrawer drawer in RegisterGroups())
            {
                drawer.isExpanded = Values.TryGetValue($"{suffix}/{drawer.Title}", out bool value) && value;
                groups.Add(drawer);
            }

            buttons = new List<(string, MethodInfo)>();
            BindingFlags flags = BindingFlags.Static | BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (MethodInfo methodInfo in target.GetType().GetMethods(flags))
            {
                var inspectorButton = methodInfo.GetCustomAttribute<InspectorButton>();
                if (inspectorButton != null)
                {
                    if (methodInfo.IsGenericMethod)
                    {
                        Debug.LogError($"Method \"{methodInfo.Name}\" is generic and can't be shown in inspector.");
                        continue;
                    }

                    if (methodInfo.GetParameters().Length > 0)
                    {
                        Debug.LogError($"Method \"{methodInfo.Name}\" has parameters and can't be shown in inspector.");
                        continue;
                    }

                    if (string.IsNullOrEmpty(inspectorButton.text)) buttons.Add((methodInfo.Name, methodInfo));
                    else buttons.Add((inspectorButton.text, methodInfo));
                }
            }
        }

        protected virtual void OnDisable()
        {
            string suffix = target.GetType().Name;
            foreach (BasePropertyGroupDrawer group in groups)
            {
                Values[$"{suffix}/{group.Title}"] = group.isExpanded;
            }
        }

        public override void OnInspectorGUI()
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.PropertyField(scriptProperty);
            EditorGUI.BeginChangeCheck();
            GUI.enabled = guiEnabled;

            foreach (BasePropertyGroupDrawer group in groups)
            {
                group.Draw();
            }

            EditorGUILayout.Space(20);

            foreach ((string, MethodInfo) button in buttons)
            {
                if (GUILayout.Button(button.Item1))
                {
                    if (button.Item2 != null)
                    {
                        button.Item2.Invoke(target, null);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UltimateSaveSystem
{
    [DefaultExecutionOrder(-10)]
    public sealed class SavedObject : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private string _guid;
        [SerializeField] private List<ComponentInfo> _components;

        private void Reset()
        {
            _guid = Guid.NewGuid().ToString();
            GetAllComponents();
        }

        private void Awake()
        {
            Load();
        }

        private void OnEnable()
        {
            USSManager.OnSave += Save;
        }

        private void OnDisable()
        {
            USSManager.OnSave -= Save;
        }

        private void OnDestroy()
        {
            try
            {
                Save();
            }
            catch
            {
            }
        }

        private void GetAllComponents()
        {
            if (gameObject == null) return;
            
            int savedIndex = 0;
            _components = new List<ComponentInfo>();
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component == null) continue;
                if (TypesLibrary.Contains(component.GetType()))
                {
                    _components.Add(new ComponentInfo(savedIndex, component));
                    savedIndex++;
                }
            }
        }
        
        private string GetComponentGuid(ComponentInfo component) => $"{_guid}|[{component.SavedIndex}]";

        private void Save()
        {
            Dictionary<string, object> globals = new Dictionary<string, object>();
            foreach (var componentInfo in _components)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                var typeHolder = TypesLibrary.GetFields(componentInfo.Component.GetType());
                foreach (FieldInfo fieldInfo in typeHolder.localFields)
                    data[fieldInfo.Name] = fieldInfo.GetValue(componentInfo.Component);

                foreach ((string id, FieldInfo fieldInfo) in typeHolder.globalFields)
                {
                    if (globals.ContainsKey(id))
                        Debug.LogError($"Object contains multiple global fields with same id: `{id}`");
                    else
                        globals[id] = fieldInfo.GetValue(componentInfo.Component);
                }

                string componentGuid = GetComponentGuid(componentInfo);
                if (data.Count > 0) USSManager.SetObjectData(componentGuid, data);
                else USSManager.ClearObjectData(componentGuid);
            }

            if (globals.Count > 0) USSManager.UpdateGlobals(globals);
        }

        private void Load()
        {
            foreach (var componentInfo in _components)
            {
                string componentGuid = GetComponentGuid(componentInfo);
                Dictionary<string, object> data = USSManager.GetObjectData(componentGuid);
                if (data == null)
                    continue;

                var typeHolder = TypesLibrary.GetFields(componentInfo.Component.GetType());
                foreach (FieldInfo fieldInfo in typeHolder.localFields)
                {
                    if (data.TryGetValue(fieldInfo.Name, out object value)) 
                        SetFieldValue(componentInfo.Component, fieldInfo, value);
                }

                foreach ((string fieldID, FieldInfo fieldInfo) in typeHolder.globalFields)
                {
                    if (USSManager.TryGetGlobalValue(fieldID, out object value)) 
                        SetFieldValue(componentInfo.Component, fieldInfo, value);
                }
            }
        }

        private static void SetFieldValue(object component, FieldInfo fieldInfo, object value)
        {
            Type fieldType = fieldInfo.FieldType;
            if (fieldType == typeof(int))
            {
                if (value is int)
                    fieldInfo.SetValue(component, value);
                else
                    fieldInfo.SetValue(component, (int)(long)value);
            }
            else if (fieldType == typeof(float))
            {
                if (value is float)
                    fieldInfo.SetValue(component, (float)value);
                else
                    fieldInfo.SetValue(component, (float)(double)value);
            }
            else if (fieldType == typeof(bool))
                fieldInfo.SetValue(component, (bool)value);
            else if (fieldType == typeof(string))
                fieldInfo.SetValue(component, (string)value);
            else if (value is JObject jObject)
                fieldInfo.SetValue(component, jObject.ToObject(fieldType));
        }

        public void OnBeforeSerialize()
        {
            GetAllComponents();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
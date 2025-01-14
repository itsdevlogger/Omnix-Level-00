using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UltimateSaveSystem
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class TypesLibrary
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static Dictionary<Type, TypeHolder> _types = new Dictionary<Type, TypeHolder>();

        static TypesLibrary()
        {
#if UNITY_EDITOR
            UpdateTypesDictionary();
            SaveCache();
#else
            LoadCache();
#endif
        }

#if UNITY_EDITOR
        private static void UpdateTypesDictionary()
        {
            _types.Clear();

            var globalFieldsNames = new Dictionary<string, (Type, string)>(); // id: (type, path)
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(Component).IsAssignableFrom(type)); // Derives from Component

            foreach (var type in allTypes)
            {
                var allFields = type.GetFields(BINDING_FLAGS);
                var localFields = new List<FieldInfo>();
                var globalFields = new Dictionary<string, FieldInfo>();
                bool hasGlobalFields = false;
                foreach (var field in allFields)
                {
                    var attribute = field.GetCustomAttribute<SavedAttribute>();
                    if (attribute == null)
                        continue;

                    if (string.IsNullOrEmpty(attribute.globalId))
                    {
                        localFields.Add(field);
                        continue;
                    }

                    var fieldDetails = $"{type.FullName}.{field.Name}";
                    if (globalFieldsNames.TryGetValue(attribute.globalId, out var existingField) &&
                        field.FieldType != existingField.Item1)
                    {
                        Debug.LogError(
                            $"Type Mismatch for global ID `{attribute.globalId}`: `{existingField.Item1.FullName}` (path: {existingField.Item2}) != `{field.FieldType.FullName}` (path: {fieldDetails}).");
                    }
                    else
                    {
                        hasGlobalFields = true;
                        globalFields.Add(attribute.globalId, field);
                        globalFieldsNames[attribute.globalId] = (field.FieldType, fieldDetails);
                    }
                }

                if (localFields.Count > 0 || hasGlobalFields)
                    _types[type] = new TypeHolder(localFields, globalFields);
            }
        }

        private static void SaveCache()
        {
#if UNITY_EDITOR
            var cachedTypes = new List<CachedType>();
            foreach ((var type, TypeHolder holder) in _types)
            {
                var ct = new CachedType();
                ct.TypeName = type.AssemblyQualifiedName;
                ct.LocalFields = holder.localFields.Select(f => f.Name).ToList();
                ct.GlobalFields = holder.globalFields.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name);
                cachedTypes.Add(ct);
            }

            var cache = new TypesCache
            {
                CachedTypes = cachedTypes
            };

            var json = JsonUtility.ToJson(cache, true);
            var path = "Assets/Resources/TypesLibraryCache.json";
            System.IO.File.WriteAllText(path, json);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif

        private static void LoadCache()
        {
            var textAsset = Resources.Load<TextAsset>("TypesLibraryCache");
            if (textAsset == null)
                return;

            var cache = JsonUtility.FromJson<TypesCache>(textAsset.text);
            _types = new Dictionary<Type, TypeHolder>();

            foreach (var cachedType in cache.CachedTypes)
            {
                var type = Type.GetType(cachedType.TypeName);
                if (type == null)
                {
                    Debug.LogError($"Type `{cachedType.TypeName}` could not be found.");
                    continue;
                }

                if ((cachedType.LocalFields == null || cachedType.LocalFields.Count == 0) &&
                    (cachedType.GlobalFields == null || cachedType.GlobalFields.Count == 0))
                    continue;


                Dictionary<string, FieldInfo> allFields = new Dictionary<string, FieldInfo>();
                foreach (FieldInfo info in type.GetFields(BINDING_FLAGS))
                    allFields[info.Name] = info;

                List<FieldInfo>
                    localFields =
                        new List<FieldInfo>(); //cachedType.LocalFields.Select(fieldName => type.GetField(fieldName, BINDING_FLAGS)).ToList();
                Dictionary<string, FieldInfo>
                    globalFields =
                        new Dictionary<string, FieldInfo>(); // cachedType.GlobalFields.ToDictionary(kvp => kvp.Key, kvp => type.GetField(kvp.Value, BINDING_FLAGS));

                if (cachedType.LocalFields != null)
                {
                    foreach (string typeName in cachedType.LocalFields)
                    {
                        if (allFields.TryGetValue(typeName, out FieldInfo fieldInfo))
                            localFields.Add(fieldInfo);
                    }
                }

                if (cachedType.GlobalFields != null)
                {
                    foreach ((string typeId, string typeName) in cachedType.GlobalFields)
                    {
                        if (allFields.TryGetValue(typeName, out FieldInfo fieldInfo))
                            globalFields.Add(typeId, fieldInfo);
                    }
                }


                if (localFields.Count > 0 || globalFields.Count > 0)
                    _types[type] = new TypeHolder(localFields, globalFields);
            }
        }

        public static TypeHolder GetFields(Type type)
        {
            if (_types.TryGetValue(type, out TypeHolder typeHolder))
                return typeHolder;
            return TypeHolder.EMPTY;
        }

        public static bool Contains(Type type)
        {
            return _types.ContainsKey(type);
        }
    }

    public class TypeHolder
    {
        public static readonly TypeHolder EMPTY =
            new TypeHolder(new List<FieldInfo>(), new Dictionary<string, FieldInfo>());

        public readonly List<FieldInfo> localFields;
        public readonly Dictionary<string, FieldInfo> globalFields;

        public TypeHolder(List<FieldInfo> localFields, Dictionary<string, FieldInfo> globalFields)
        {
            this.localFields = localFields;
            this.globalFields = globalFields;
        }
    }

    [Serializable]
    public class TypesCache
    {
        public List<CachedType> CachedTypes;
    }

    [Serializable]
    public class CachedType
    {
        public string TypeName;
        public List<string> LocalFields; // name
        public Dictionary<string, string> GlobalFields; // ID: name
    }
}
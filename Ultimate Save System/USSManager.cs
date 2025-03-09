using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace UltimateSaveSystem
{
    public class USSManager : MonoBehaviour
    {
        public static USSManager Instance { get; private set; }
        public static event Action OnSave;
        private static string _savePath;
        private static string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(_savePath))
                    _savePath = Path.Combine(Application.persistentDataPath, "Save");
                return _savePath;
            }
        }

        [SerializeField] private string _profile;
        private GameData _gameData;

        private void Awake()
        {
            Debug.Log("Creating USS Manager");
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                LoadProfile(_profile);
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                SaveProfile(_profile);
        }

        [ContextMenu("Save Profile")]
        private void SaveProfile()
        {
            SaveProfile(_profile);
        }

        [ContextMenu("Delete All Profiles")]
        private void DeleteAllProfiles()
        {
            
        }

        public static void SaveProfile(string profile)
        {
            OnSave?.Invoke();

            if (Directory.Exists(SavePath) == false)
                Directory.CreateDirectory(SavePath);

            if (string.IsNullOrEmpty(profile))
                profile = DateTime.Now.ToString("yyyyMMdd HHmmss");

            string path = Path.Join(SavePath, $"{profile}.bin");
            string data = Instance._gameData.Serialize();
            File.WriteAllText(path, data);
            Instance._profile = profile;
        }

        public static void LoadProfile(string profile)
        {
            if (Directory.Exists(SavePath) == false)
                Directory.CreateDirectory(SavePath);

            if (string.IsNullOrEmpty(profile))
            {
                Instance._gameData = new GameData();
                Instance._profile = profile;
                return;
            }

            string path = Path.Join(SavePath, $"{profile}.bin");
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                Instance._gameData = data.Deserialize<GameData>();
                Instance._profile = profile;
            }
            else
            {
                Instance._gameData = new GameData();
                Instance._profile = profile;
            }
        }

        public static bool TryGetGlobalValue(string id, out object value)
        {
            return Instance._gameData.globalFields.TryGetValue(id, out value);
        }

        public static void UpdateGlobal(string id, object value)
        {
            if (!Instance._gameData.globalFields.TryGetValue(id, out object currentValue)) 
                Instance._gameData.globalFields.Add(id, value);
            else if (ValidateType(value, currentValue))
                Instance._gameData.globalFields[id] = value;
            else
                Debug.LogError($"Type Mismatch for field (id:{id}): (type: {currentValue.GetType().FullName}) != (type: {value.GetType().FullName}).");
            return;
            
            bool ValidateType(object obj1, object obj2)
            {
                if (obj1 == null || obj2 == null) return true;
                var obj1Type = obj1.GetType();
                var obj2Type = obj2.GetType();
                if (obj1Type == obj2Type) return true;
                if (obj1Type == typeof(int))
                    return obj2Type == typeof(long) || obj2Type == typeof(short);
                if (obj1Type == typeof(float))
                    return obj2Type == typeof(double) || obj2Type == typeof(decimal);
                return false;
            }
        }
        
        public static void UpdateGlobals(Dictionary<string,object> globals)
        {
            foreach ((string fieldId, object fieldValue) in globals) 
                UpdateGlobal(fieldId, fieldValue);
        }
        
        public static Dictionary<string, object> GetObjectData(string guid)
        {
            if (Instance._gameData.components.TryGetValue(guid, out Dictionary<string, object> data))
                return data;
            return new Dictionary<string, object>();
        }

        public static void SetObjectData(string guid, Dictionary<string, object> data)
        {
            Instance._gameData.components[guid] = data;
        }

        public static void ClearObjectData(string guid)
        {
            Instance._gameData.components.Remove(guid);
        }
    }


    [Serializable]
    public class GameData
    {
        public Dictionary<string, object> globalFields = new(); // {id: value}
        public Dictionary<string, Dictionary<string, object>> components = new();   // {guid: {fieldName: fieldValue}}
    }
}
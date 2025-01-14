using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace UltimateSaveSystem
{
    public class USSManager : MonoBehaviour
    {
        private static string SAVE_PATH;
        private static USSManager _instance;
        public static event Action OnSave;

        public static USSManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject("[USS Manager]").AddComponent<USSManager>();
                return _instance;
            }
        }

        [SerializeField] private string _profile;
        private GameData _gameData;
        

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                SAVE_PATH = Application.persistentDataPath;
                _instance = this;
                LoadProfile(_profile);
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
                SaveProfile(_profile);
        }

        [ContextMenu("Save Profile")]
        private void SaveProfile()
        {
            SaveProfile(_profile);
        }

        public static void SaveProfile(string profile)
        {
            OnSave?.Invoke();

            if (Directory.Exists(SAVE_PATH) == false)
                Directory.CreateDirectory(SAVE_PATH);

            if (string.IsNullOrEmpty(profile))
                profile = DateTime.Now.ToString("yyyyMMdd HHmmss");

            string path = Path.Join(SAVE_PATH, $"{profile}.bin");
            string data = Instance._gameData.Serialize();
            File.WriteAllText(path, data);
            _instance._profile = profile;
        }

        public static void LoadProfile(string profile)
        {
            if (Directory.Exists(SAVE_PATH) == false)
                Directory.CreateDirectory(SAVE_PATH);

            if (string.IsNullOrEmpty(profile))
            {
                Instance._gameData = new GameData();
                _instance._profile = profile;
                return;
            }

            string path = Path.Join(SAVE_PATH, $"{profile}.bin");
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                Instance._gameData = data.Deserialize<GameData>();
                _instance._profile = profile;
            }
            else
            {
                Instance._gameData = new GameData();
                _instance._profile = profile;
            }
        }

        public static bool TryGetGlobalValue(string id, out object value)
        {
            return Instance._gameData.globalFields.TryGetValue(id, out value);
        }

        public static void UpdateGlobals(Dictionary<string,object> globals)
        {
            foreach ((string fieldId, object fieldValue) in globals)
            {
                if (!Instance._gameData.globalFields.TryGetValue(fieldId, out object currentValue)) 
                    Instance._gameData.globalFields.Add(fieldId, fieldValue);
                else if (fieldValue == null || currentValue == null || fieldValue.GetType() == currentValue.GetType())
                    Instance._gameData.globalFields[fieldId] = fieldValue;
                else
                    Debug.LogError($"Type Mismatch for field (id:{fieldId}): (type: {currentValue.GetType().FullName}) != (type: {fieldValue.GetType().FullName}).");
            }   
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
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ThemedUi
{
    public abstract class EditorStorage<T> : ScriptableObject 
        where T: EditorStorage<T>
    {
        private static string StorageName => typeof(T).Name;
        private static T INSTANCE;

        public static T Instance
        {
            get
            {
                if (INSTANCE == null) LoadOrCreateStorage();
                return INSTANCE;
            }
        }

        private static void LoadOrCreateStorage()
        {
            INSTANCE = Resources.Load<T>(StorageName);
            if (INSTANCE != null) return;

            string resourcesFolderPath = "Assets/Editor/Resources/";
            string storagePath = resourcesFolderPath + StorageName + ".asset";
            if (!Directory.Exists(resourcesFolderPath))
            {
                Directory.CreateDirectory(resourcesFolderPath);
                AssetDatabase.ImportAsset(resourcesFolderPath);
            }

            INSTANCE = CreateInstance<T>();
            if (INSTANCE == null)
            {
                Debug.LogError("Not able to load Omnix Storage. Editor windows wont work without it. Try restarting.");
                return;
            }

            AssetDatabase.CreateAsset(INSTANCE, storagePath);
            AssetDatabase.ImportAsset(storagePath);
            INSTANCE.Init();
            EditorUtility.SetDirty(INSTANCE);
        }

        protected abstract void Init();

        protected void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }
}
using System;
using JetBrains.Annotations;
using Omnix.DesignPatterns;
using Omnix.Notification;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Omnix.SceneManagement
{
    public static class SceneLoader
    {
        public static event Action<SceneId> OnSceneLoadingStarts;
        public static event Action<SceneId> OnSceneLoadingEnds;
        [CanBeNull] public static AsyncOperation LoadingOperation { get; private set; }
        public static SceneId CurrentlyLoading { get; private set; } = SceneId.Unknown;

        private static TaskQueue queue;

        private static void OnLoadComplete(Scene scene, LoadSceneMode _)
        {
            SceneId sceneId = scene.name.GetId();
            OnSceneLoadingEnds?.Invoke(sceneId);
            
            if (CurrentlyLoading != SceneId.Unknown && sceneId == CurrentlyLoading)
            {
                LoadingOperation = null;
                CurrentlyLoading = SceneId.Unknown;
                NotiCon.HideLoading();
                queue.TaskDone();
            }
        }

        public static void Load(this SceneId id, LoadSceneMode mode = LoadSceneMode.Single, bool isAsync = true)
        {
            if (id == SceneId.Unknown)
            {
                Debug.LogError($"Cannot load scene with id {id}");
                return;
            }
            
            Debug.Log($"Loading scene: {id}");
            if (queue == null)
            {
                queue = new TaskQueue();
                SceneManager.sceneLoaded += OnLoadComplete;
            }
            
            queue.BeginTask(TaskLoad, id, mode, isAsync);
        }
        
        private static void TaskLoad(SceneId id, LoadSceneMode mode, bool isAsync)
        {
            // Checks
            if (id == SceneId.Unknown)
            {
                Debug.LogError("Trying to load unknown scene");
                return;
            }

            string sceneName = id.GetName();
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError($"Unable to find scene with id: {id}");
                return;
            }

            // Setup
            CurrentlyLoading = id;
            OnSceneLoadingStarts?.Invoke(id);

            // Load
            NotiCon.ShowLoading();
            NotiCon.LoadingText = $"Loading Scene {sceneName}";
            if (isAsync) LoadingOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            else SceneManager.LoadScene(sceneName, mode);
        }
    }
}
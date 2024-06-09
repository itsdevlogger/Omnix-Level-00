using UnityEngine.SceneManagement;

namespace Omnix.SceneManagement
{
    [System.Serializable]
    public class SceneProperties
    {
        public SceneId scene;
        public LoadSceneMode mode;
        public bool isAsync;

        public SceneProperties(string name, LoadSceneMode mode = LoadSceneMode.Single, bool isAsync = true)
        {
            this.scene = name.GetId();
            this.mode = mode;
            this.isAsync = isAsync;
        }
        
        public SceneProperties(SceneId scene, LoadSceneMode mode = LoadSceneMode.Single, bool isAsync = true)
        {
            this.scene = scene;
            this.mode = mode;
            this.isAsync = isAsync;
        }

        public void Load() => scene.Load(mode, isAsync);
    }
}
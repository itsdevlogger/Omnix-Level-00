using UnityEngine.SceneManagement;

namespace Omnix.SceneManagement
{
    [System.Serializable]
    public class SceneProperties
    {
        public SceneId scene;
        public LoadSceneMode mode;
        public bool isAsync;

        public SceneProperties(string name)
        {
            scene = name.GetId();
            mode = LoadSceneMode.Single;
            isAsync = true;
        }
        
        public SceneProperties(SceneId scene)
        {
            this.scene = scene;
            mode = LoadSceneMode.Single;
            isAsync = true;
        }

        public void Load() => scene.Load(mode, isAsync);
    }
}